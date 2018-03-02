using System;
using System.Collections.Concurrent;

namespace AngOE.Common.CodeSection
{
    public abstract class BaseCodeSection : IDisposable
    {
        private string nameSectionType;

        protected BaseCodeSection()
        {
            nameSectionType = GetType().Name;
        }

        private class RefInt
        {
            public int Int = 0;
        }

        /// <summary>
        /// Help to store nesting levels for all possible subclasses of BaseCodeSection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static class Statics<T> where T : BaseCodeSection
        {
            public static ConcurrentDictionary<int, RefInt> dictionarySectionNestLevel = new ConcurrentDictionary<int, RefInt>();
            public static ConcurrentDictionary<int, dynamic> dictionaryRootSection = new ConcurrentDictionary<int, dynamic>();
        }

        private static ConcurrentDictionary<int, RefInt> GetDictionarySectionNestLevel<T>(T dummy)
            where T : BaseCodeSection
        {
            return Statics<T>.dictionarySectionNestLevel;
        }

        private ConcurrentDictionary<int, RefInt> GetDictionarySectionNestLevel()
        {
            dynamic dummy = this;
            return GetDictionarySectionNestLevel(dummy);
        }

        private static ConcurrentDictionary<int, dynamic> GetDictionaryRootSection<T>(T dummy)
            where T : BaseCodeSection
        {
            return Statics<T>.dictionaryRootSection;
        }

        private ConcurrentDictionary<int, dynamic> GetDictionaryRootSection()
        {
            dynamic dummy = this;
            return GetDictionaryRootSection(dummy);
        }

        /// <summary>
        /// Stars a section from the subclass
        /// </summary>
        protected void BeginSection()
        {
            // Check if this is a nested invokation of SessionSection 
            int nestLevel = 0;
            var dictionarySectionNestLevel = GetDictionarySectionNestLevel();
            if (!dictionarySectionNestLevel.ContainsKey(System.Threading.Thread.CurrentThread.ManagedThreadId))
            {
                dictionarySectionNestLevel[System.Threading.Thread.CurrentThread.ManagedThreadId] = new RefInt();
            }

            nestLevel = dictionarySectionNestLevel[System.Threading.Thread.CurrentThread.ManagedThreadId].Int;

            if (nestLevel > 0)
            {
                OnNestedStarted(nestLevel);
            }
            else
            {
                var dictionaryRootSection = GetDictionaryRootSection();
                dictionaryRootSection[System.Threading.Thread.CurrentThread.ManagedThreadId] = this;
                OnStarted();
            }

            // Increment the nest level count
            dictionarySectionNestLevel[System.Threading.Thread.CurrentThread.ManagedThreadId].Int++;
        }

        /// <summary>
        /// Ends the section
        /// </summary>
        public void Dispose()
        {
            // Decrement the nest level count
            var dictionarySectionNestLevel = GetDictionarySectionNestLevel();
            dictionarySectionNestLevel[System.Threading.Thread.CurrentThread.ManagedThreadId].Int--;

            // Check if we have reached the top most disposal
            int nestLevel = dictionarySectionNestLevel[System.Threading.Thread.CurrentThread.ManagedThreadId].Int;
            if (nestLevel == 0)
            {
                OnEnded();
            }
            else
            {
                OnNestedEnded(nestLevel);
            }
        }

        /// <summary>
        /// Called when the top level section has started
        /// </summary>
        protected virtual void OnStarted()
        {

        }

        /// <summary>
        /// Called when a nested section has started
        /// </summary>
        /// <param name="level"></param>
        protected virtual void OnNestedStarted(int level)
        {

        }

        /// <summary>
        /// Called when the top level section has ended
        /// </summary>
        protected virtual void OnEnded()
        {

        }

        /// <summary>
        /// Called when a nested section has ended
        /// </summary>
        /// <param name="level"></param>
        protected virtual void OnNestedEnded(int level)
        {

        }

        /// <summary>
        /// Says if the calling thread is running with a section of the provided type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static bool IsThreadInSection<T>()
            where T : BaseCodeSection
        {
            var dictionarySectionNestLevel = GetDictionarySectionNestLevel<T>(null);
            if (!dictionarySectionNestLevel.ContainsKey(System.Threading.Thread.CurrentThread.ManagedThreadId))
            {
                return false;
            }
            else
            {
                return dictionarySectionNestLevel[System.Threading.Thread.CurrentThread.ManagedThreadId].Int > 0;
            }
        }

        protected static T GetSectionRoot<T>()
            where T : BaseCodeSection
        {
            var dictionaryRootSection = GetDictionaryRootSection<T>(null);
            if (!dictionaryRootSection.ContainsKey(System.Threading.Thread.CurrentThread.ManagedThreadId))
            {
                return default(T);
            }
            else
            {
                return dictionaryRootSection[System.Threading.Thread.CurrentThread.ManagedThreadId];
            }
        }
    }
}