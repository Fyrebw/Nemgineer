using On.RoR2;
using RoR2;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zio.FileSystems;

namespace NemgineerMod
{
    internal class LanguageTokens
    {
        public static SubFileSystem fileSystem;

        internal static string languageRoot => System.IO.Path.Combine(LanguageTokens.assemblyDir, "language");

        internal static string assemblyDir => System.IO.Path.GetDirectoryName(NemgineerPlugin.pluginInfo.Location);

        public LanguageTokens() => LanguageTokens.RegisterLanguageTokens();

        public static void RegisterLanguageTokens() => Language.SetFolders += new Language.hook_SetFolders((object)null, __methodptr(fixme));

        private static void fixme(
          Language.orig_SetFolders orig,
          Language self,
          IEnumerable<string> newFolders)
        {
            if (Directory.Exists(LanguageTokens.languageRoot))
            {
                IEnumerable<string> second = Directory.EnumerateDirectories(System.IO.Path.Combine(new string[1]
                {
          LanguageTokens.languageRoot
                }), self.name);
                orig.Invoke(self, newFolders.Union<string>(second));
            }
            else
                orig.Invoke(self, newFolders);
        }
    }
}
