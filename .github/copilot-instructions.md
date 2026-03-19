# Copilot instructions for JLOrdaz.BulkToSQL

## Build, pack, and test

- Build the library with `dotnet build JLOrdaz.BulkToSQL.sln -c Release`.
- `GeneratePackageOnBuild` is enabled, so `dotnet build` also produces the NuGet package in `JLOrdaz.BulkToSQL\bin\Release\`.
- Pack explicitly with `dotnet pack JLOrdaz.BulkToSQL.sln -c Release --no-build` if you already built it.
- There is currently no test project in this repository. If tests are added later, run one test with:
  `dotnet test <test-project>.csproj --filter FullyQualifiedName~Namespace.ClassName.TestName`

## High-level architecture

- This is a small .NET class library that wraps SQL Server bulk inserts.
- `BulkInsert<T>` is the main entry point. It is constructed with a `SqlConnection` and a `List<Mapeo>` that defines column-to-column mappings.
- `PutDataIntoDB` and `PutDataIntoDBAsync` convert the input collection to a `DataTable`, optionally truncate the destination table, and write rows through `SqlBulkCopy`.
- `ListToDatatable` is an internal extension class that turns `IEnumerable<T>` into a `DataTable` by reflecting over public properties.
- `IBulkInsert<T>` defines the public contract, and `Mapeo` is the simple origin/destination mapping type used by `SqlBulkCopy.ColumnMappings`.

## Repository conventions

- Keep the public namespace `JLOrdaz.BulkToSQL` and preserve the existing public API names (`BulkInsert<T>`, `IBulkInsert<T>`, `Mapeo`, `LimpiarTablaTemporal`).
- The codebase already mixes Spanish and English identifiers; follow the existing naming instead of renaming the API for consistency.
- `Mapeo.Origen` and `Mapeo.Destino` are used as explicit bulk-copy mappings, so table and column names are expected to match the caller-provided SQL schema.
- `LimpiarTablaTemporal` and the truncate paths use raw SQL object names. Treat destination and temporary table names as trusted inputs and keep that behavior aligned with the current API.
- `ListToDatatable.ToDataTable<T>` relies on `TypeDescriptor`, so it only sees public properties and emits nullable property types using their underlying CLR type.
- The project targets `net8.0;net10.0`, uses `Microsoft.Data.SqlClient`, nullable reference types, and package generation on build. Keep `JLOrdaz.BulkToSQL.csproj` metadata and version fields numeric and valid for SDK-style builds.
