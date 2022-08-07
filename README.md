# Shuttle.Core.PipelineTransaction

Provides a pipeline observer to handle transaction scopes.

```c#
services.AddPipelineProcessing(builder => {
    builder.AddAssembly(assembly);
    builder.AddTransactions();
});
```

