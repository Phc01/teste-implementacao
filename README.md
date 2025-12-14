# MCP – Assistente de Consulta de Produtos



Este projeto demonstra um **assistente de consulta de produtos** utilizando:

**Visual Studio 2026**

**.NET (C#)** para API e cliente

**Ollama (LLM local)** para interpretação da intenção do usuário

* Comunicação via **HTTP REST**



O objetivo é mostrar, de forma didática, como um modelo de linguagem pode ser usado **apenas como interpretador de intenção**, sem acesso direto aos dados, mantendo a lógica de negócio sob controle da aplicação.



---



## 📦 Estrutura do Projeto



```

Teste/

├── mcp-api/        # API REST de produtos

│   └── Controllers

│       └── ProdutoController.cs

│

├── mcp-cliente/    # Cliente console (assistente)

│   └── Program.cs

│

└── teste-implementacao.sln

```



---



## 🚀 Como funciona



1. O usuário faz uma pergunta em linguagem natural no **cliente console**

2. A pergunta é enviada ao **Ollama**

3. O modelo retorna **somente um JSON** contendo:



&nbsp;  * `categoria`

&nbsp;  * `listarTodos`

4. O cliente decide qual endpoint chamar na API

5. A **API retorna os produtos**, aplicando filtros quando necessário



O modelo **não acessa dados**, **não faz consultas** e **não decide regras de negócio**.



---



## 🧠 Exemplo de Perguntas Suportadas



```text

liste todos os produtos

liste todos os produtos da categoria eletronicos

liste todos os produtos da categoria cozinha

```



Perguntas fora do escopo retornam:



```text

Não faço esse tipo de busca.

```



---



## 🔌 API de Produtos



### Endpoint



```

GET /produtos

GET /produtos?categoria=Eletronicos

```



### Exemplo de Resposta



```json

{

 "categoria": "Eletronicos",

 "total": 5,

 "itens": [

   {

     "id": 1,

     "nome": "Notebook",

     "categoria": "Eletronicos",

     "preco": 3000

   }

 ]

}

```



---



## 🛠️ Como executar



### 1️⃣ Subir a API



```bash

cd mcp-api

dotnet run

```



A API ficará disponível em:



```

http://localhost:5007/produtos

```



### 2️⃣ Executar o cliente



Em outro terminal:



```bash

cd mcp-cliente

dotnet run

```



---



## 🧪 Observações Importantes



* Sempre que alterar a API, **reinicie o projeto da API**
  
* O cliente depende da API rodando corretamente na porta configurada
  
* Categorias são **case-insensitive**, mas devem bater com os valores cadastrados



---



## 🎯 Objetivo Didático



Este projeto foi criado para demonstrar:


* Uso de LLMs como **interpretadores de intenção**

* Separação clara entre IA e regras de negócio

* Evitar que a IA "invente" dados

* Controle total da aplicação sobre o que é retornado



---



## 🔮 Possíveis Melhorias Futuras



* Criar um **CategoriaController** e trabalhar com **IDs de categoria** em vez de nomes

* Persistir produtos e categorias em um banco de dados

* Implementar paginação real (skip/take)

* Criar um cache para respostas frequentes

* Adicionar testes automatizados (unitários e de integração)

* Padronizar categorias com enum ou tabela dedicada

* Criar versionamento da API

* Implementar autenticação e autorização

* Criar uma interface web ou frontend simples



---



## 📄 Licença



Projeto de estudo e demonstração técnica, sem fins comerciais.



