# Safra Report Generator - Time 5

## Aspectos técnicos

### Sobre

Este sistema tem como objetivo a geração de um relatório para demonstrar a saúde financeira de um determinado cliente. A solução realiza consultas de transações providas pela API do Banco Safra (3 transações), o resultado das consultas é inserido num banco de dados local. 
Após essa primeira etapa é realizada a execução do código para clusterização escrito em python (mais informações em: https://github.com/AlexandreCodes/TechneeSafra_Time5). Com a execução do script anterior, o banco de dados local é atualizado com o resultado da clusterização.
Uma vez obtidos os dados, estes são inseridos em uma tabela .xslx e então são enviados em um e-mail para os clientes do banco. 

###

Para a execução do programa executar o arquivo no diretorio em que o projeto foi salvo SafraHackathon\Report_Generator_V1\bin\Debug\Report_Generator_V1.exe

### Estrutura

A solução está estruturada nas pastas:

#### Model

Contém a classe referente aos clientes do banco Safra (Client), a classe referente ao banco de dados local (Database), a classe referente à comunicação com as APIs (Communication), as classes referentes ao relatório (Report) e a classe referente ao .json recebido como resposta da API (TransactionData).

#### Communication

Contém a classe referente ao banco de dados local.

#### Cluster

Contém a classe referente ao script de agrupamento (cluter).

### Bibliotecas utilizadas

System;
System.Collections.Generic;
System.Threading.Tasks;
System.Configuration;
System.IO;
System.Diagnostics;
Newtonsoft.Json;
RestSharp;
MySql.Data.MySqlClient;
System.Text;
System.Net;
System.Net.Mail;
Microsoft.Office.Interop.Excel;
System.Drawing;
System.Linq;

### Disclaimer

Isso não é um produto oficial do banco Safra.

A previsão de Saúde financeira de clientes é uma projeto utilizando um dataset financeiro sintético para treino, fruto de um hackathon promovido pelo banco safra para o processo seletivo de Technee.

### Time 5

- Alexandre França
- Guilhemo Valdez
- Luiz Guilherme Motta
- Nicolas Sathler de Araujo
- Rodrigo Coimbra


### Issues no GitHub

Podem ser abertos [Issues](https://github.com/rodcoimbra/SafraHackathon) no GitHub do Time 5. 


