using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Thor.Analyzers;


[Generator(LanguageNames.CSharp)]
public partial class ServiceRegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 注册一个增量生成器
        var provider = context.CompilationProvider.Combine(context.AnalyzerConfigOptionsProvider);
        context.RegisterSourceOutput(provider, (spc, source) => Execute(source.Left, spc));
    }

    private void Execute(Compilation compilation, SourceProductionContext context)
    {
        var scopeMethods = new List<string>();
        var singletonMethods = new List<string>();
        var transientMethods = new List<string>();

        // 扫描当前编译单元和所有引用的程序集
        ScanService.ScanAndCollect(compilation, scopeMethods, singletonMethods, transientMethods);

        var scopeRegistrations = string.Join("\n", scopeMethods.Select(m => $"services.AddScoped<{m}>();\n            "));
        var singletonRegistrations = string.Join("\n", singletonMethods.Select(m => $"services.AddSingleton<{m}>();\n            "));
        var transientRegistrations = string.Join("\n", transientMethods.Select(m => $"services.AddTransient<{m}>();\n            "));

        // 生成源代码
        string source = $@"// Gnarly自动生成的源代码，请勿手动修改
using System;
using Microsoft.Extensions.DependencyInjection;

namespace System
{{
    public static class GnarlyExtensions
    {{
        /// <summary>
        /// SourceGenerator自动注入服务
        /// </summary>
        public static IServiceCollection AddAutoGnarly(this IServiceCollection services)
        {{
            {scopeRegistrations}
            {singletonRegistrations}
            {transientRegistrations}
            return services;
        }}
    }}
}}
";

        // 添加生成的源代码到编译单元
        context.AddSource($"GnarlyExtensions.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}