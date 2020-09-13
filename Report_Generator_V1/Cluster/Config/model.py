#!/usr/bin/env python
# coding: utf-8

# ------------------------------------------------------------
# Carga do modelo, conexão com o banco,
# e execução e output da classificação de volta para o banco
# ### Equipe 5 - Safra Hackers
# ------------------------------------------------------------

# Declarando Bibliotecas

import mysql.connector
import pickle
import sys

from configparser import ConfigParser
from datetime import datetime
from sklearn.cluster import KMeans

# Definições de constantes

TRUNCATE_SOURCE = False if len(sys.argv) >= 2 and sys.argv[1].lower() == "false" else True
TIME_REF = datetime.now().strftime('%Y-%m-%d %H:%M')

# Declaração de funções


def get_transfers_query():
    return "SELECT conta, saida, entrada FROM safra.transferencias"


def get_insert_query():
    return """INSERT INTO 
              safra.modelo (conta, saida, entrada, cluster, execution_time) 
              VALUES ('{0}',{1},{2},'{3}','{4}')"""


def get_truncate_query():
    return "TRUNCATE TABLE safra.transferencias"


def insert_data(_cursor, _result, _clusters,
                _ldict, _execution_time, truncate=False):
    """ Inserir os resultados da clusterização no banco de dados
    se truncate = True, deleta os dados da tabela de estoque ao final da execução
    """

    for item in zip(_result, clusters):
        sql_query = get_insert_query()
        sql_query = sql_query.format(item[0][0],
                                     item[0][1],
                                     item[0][2],
                                     _ldict[str(item[1])],
                                     _execution_time)

        _cursor.execute(sql_query)
    if truncate:
        _cursor.execute(get_truncate_query())

    conn.commit()
    return


def create_connection(_host, _user, _password, _database):
    return mysql.connector.connect(
      host=_host,
      user=_user,
      password=_password,
      database=_database,
      auth_plugin='mysql_native_password'
    )


if __name__ == "__main__":

    config_object = ConfigParser()
    config_object.read("config.ini", "utf8")

    conn = create_connection(_host=config_object['CONN_STRING']["host"],
                             _user=config_object['CONN_STRING']["user"],
                             _password=config_object['CONN_STRING']["password"],
                             _database=config_object['CONN_STRING']["database"]
                             )

    # ### Leitura dos dados de conta e balanços

    print("Lendo banco da dados")
    cursor = conn.cursor()
    cursor.execute(get_transfers_query())

    result = cursor.fetchall()

    # Preparando entrada do modelo
    if result:
        kmeans_input = [[res[1], res[2]] for res in result]

        # Carga do modelo e predição

        loaded_model = pickle.load(open('Kmeans.sav', 'rb'))
        print("Classificando dados")
        clusters = loaded_model.predict(kmeans_input)

        # dicionário da indicação do kmeans

        ldict = dict(config_object._sections["CLUSTERS"])

        # Inserção dos resultados de volta na base:
        print("Inserindo dados no banco e deletando dados de entrada")
        insert_data(cursor, result, clusters, ldict, TIME_REF, truncate=TRUNCATE_SOURCE)
        print("OK")
    else:
        print("Não há dados para processamento no banco de dados")
