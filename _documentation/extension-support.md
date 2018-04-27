---
layout: documentation
title: Connect to other databases
prev_section: setup-condition
next_section: extension-installation
permalink: /docs/extension-support/
---
Natively, NBi supports connections to OleDb, ODBC and ADOMD compatible databases. With the extensions, it's now possible to reach a bunch of new databases and to query them.

At the moment, these extensions only support query execution and not structure discovery or other features such as ETL or reports.

The available extensions are enlisted here under. If you developped an extension for another database, feel free to submit a pull request to this page or to drop me a link via Twitter or email.

### Gremlin
The NBi.Core.Gremlin extension supports connection through the [Gremlin.Net driver](https://github.com/apache/tinkerpop/tree/master/gremlin-dotnet) to any Tinkerpop-enabled database including Azure Cosmos DB, Tinkerpop Server, JanusGraph and OrientDB. It lets you submit **Gremlin** queries to the server and interpret the results to transform them into a result-set.

Website: https://github.com/seddryck/nbi.gremlin/
Available on GitHub [![https://img.shields.io/github/release/Seddryck/NBi.Gremlin.svg]](https://github.com/seddryck/nbi.gremlin/releases) and Nuget ![nuget](https://img.shields.io/nuget/v/NBi.Gremlin.svg)

### Neo4j
The NBi.Core.Neo4j extension supports connection through the [Neo4j Bolt driver](https://github.com/neo4j/neo4j-dotnet-driver) to a Neo4j database. It lets you submit **Cypher** queries to the server and interpret the results to transform them into a result-set.

Website: https://github.com/seddryck/nbi.neo4j/
Available on GitHub [![https://img.shields.io/github/release/Seddryck/NBi.Neo4j.svg]](https://github.com/seddryck/nbi.neo4j/releases) and Nuget ![nuget](https://img.shields.io/nuget/v/NBi.Neo4j.svg)

### Azure Cosmos DB
The NBi.Core.CosmosDB extension supports connection through the [Graph API](https://azure.microsoft.com/en-us/resources/samples/azure-cosmos-db-graph-dotnet-getting-started/) and the [SQL API](https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-introduction) to an Azure Cosmos DB database. It lets you submit **Gremlin** or **SQL-like** queries (depending on the API) to the server and interpret the results to transform them into a result-set. Note that if you're running Gremlin queries, it's recommended to use the Gremlin.Net driver and you should use the Gremlin extension in place of the Azure Cosmos DB extension.

Website: https://github.com/seddryck/nbi.cosmosdb/
Available on GitHub [![https://img.shields.io/github/release/Seddryck/NBi.CosmosDb.svg]](https://github.com/seddryck/nbi.cosmosdb/releases) and Nuget ![nuget](https://img.shields.io/nuget/v/NBi.CosmosDb.svg)