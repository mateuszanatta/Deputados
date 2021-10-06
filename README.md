[![LinkedIn][linkedin-shield]][linkedin-url]

<h3 align="center">Deputados .NET 5</h3>
<p align="center">
    Brazilian Congress publishes an API with Congressman and Congresswoman information, such as expenses and events. This project wants to consolidate this data by extracting the relevant data from the Congress API and treating it.
</p>

## Instalation
1. Clone the repo
    ```sh
    git clone https://github.com/mateuszanatta/Deputados.git
    ```
2. Install MongoDB
3. Install MongoDB.Driver nuget package
4. (Optional) Open the appsettings.json file and edit the following lines

    ```javascript
    "DeputadosDatabaseSettings": {
      "DeputadosCollectionName": "Deputados",
      "ConnectionString": "mongodb://localhost:27017",
      "DatabaseName": "DeputadosDb"
    }
    ```
    
### ENDPOINTS

`GET: /GetDadosFromAPI` retrieves data `/deputados` and `/deputados/{id}/despesas` from [Brazilian Congress API](https://dadosabertos.camara.leg.br/swagger/api.html) and create an object with Congressman/Congresswoman information

```javascript
{
  "dados": [
    {
      "idDatabase": "string",
      "id": 0,
      "uri": "string",
      "nome": "string",
      "siglaPartido": "string",
      "uriPartido": "string",
      "siglaUf": "string",
      "idLegislatura": 0,
      "urlFoto": "string",
      "email": "string",
      "expenses": [
        {
          "ano": 0,
          "mes": 0,
          "tipoDespesa": "string",
          "tipoDocumento": "string",
          "valorDocumento": 0,
          "valorLiquido": 0,
          "valorGlosa": 0
        }
      ]
    }
  ],
  "links": [
    {
      "rel": "string",
      "href": "string"
    }
  ]
}
```

`GET: /GetDeputados` retrieve deputados saved in the MongoDB in a friendlier way

`GET: /GetDeputado/{idDeputado}` retrieve deputado by id. The id is the Congressman/Congresswoman id defined in [Brazilian Congress API](https://dadosabertos.camara.leg.br/swagger/api.html). The data retrieved is saved in the MongoDB.

```javascript
{
  "idDatabase": "string",
  "id": 0,
  "uri": "string",
  "nome": "string",
  "siglaPartido": "string",
  "uriPartido": "string",
  "siglaUf": "string",
  "idLegislatura": 0,
  "urlFoto": "string",
  "email": "string",
  "expenses": [
    {
      "ano": 0,
      "mes": 0,
      "tipoDespesa": "string",
      "tipoDocumento": "string",
      "valorDocumento": 0,
      "valorLiquido": 0,
      "valorGlosa": 0
    }
  ]
}
```

`GET: /GetDeputadosStatistics/{idDeputado}` compute and retrieve Congressman/Congresswoman statistics by its id (refer to ENDPOINT `GET: /GetDeputado/{idDeputado}`)

```javascript
{
  "expensesByYear": [
    {
      "year": 0,
      "value": 0
    }
  ],
  "expensesByMonth": [
    {
      "year": 0,
      "month": 0,
      "value": 0
    }
  ]
}
```

`POST: /Deputados` make a call to [Brazilian Congress API](https://dadosabertos.camara.leg.br/swagger/api.html), extract the relevant data, and save it on MongoDB. This is an important endpoint, since the Brazilian Congress API may be unstable, or data may be erased.

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/mateuszanatta/
