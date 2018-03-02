using System;
using System.Diagnostics;
using AngOE.Common.CodeSection;

namespace AngOE.Common
{
    public class ExecuteManager
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        internal static readonly ExecuteManager Instance = new ExecuteManager();

        private static ILogger _logger = null;
        private static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Logger();
                }

                return _logger;
            }
        }

        public static void DebugTimer(Action code, string message, params object[] args)
        {
            if (Logger.IsDebugEnabled)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                code();
                stopwatch.Stop();

                message = string.Format(message, args);
                Logger.DebugFormat("{0} | {1}ms", message, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                code();
            }
        }

        public static TResult DebugTimer<TResult>(Func<TResult> code, string message, params object[] args)
        {
            TResult result = default(TResult);
            if (Logger.IsDebugEnabled)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                result = code();
                stopwatch.Stop();

                message = string.Format(message, args);
                Logger.DebugFormat("{0} | {1}ms", message, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                result = code();
            }

            return result;
        }

        /// <summary>
        /// Helper method to encapsulate exception handling
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static void Execute(Action code)
        {
            ExecuteInternal(() =>
            {
                code();
                return null;
            });
        }

        /// <summary>
        /// Helper method to encapsulate exception handling
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TResult Execute<TResult>(Func<TResult> code)
        {
            return ExecuteInternal(() =>
            {
                dynamic result = code();
                return result;
            });
        }

        /// <summary>
        /// Helper method that swallows all exceptions if there are any
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static void ExecuteResumeOnError(Action code)
        {
            try
            {
                ExecuteInternal(() =>
                {
                    code();
                    return null;
                });
            }
            catch (Exception)
            {
                Logger.InfoFormat("EXCEPTION HAS BEEN SWALLOWED");
            }
        }

        /// <summary>
        /// Helper method that swallows all exceptions if there are any
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TResult ExecuteResumeOnError<TResult>(Func<TResult> code)
        {
            try
            {
                return ExecuteInternal(() =>
                {
                    dynamic result = code();
                    return result;
                });
            }
            catch (Exception)
            {
                Logger.InfoFormat("EXCEPTION HAS BEEN SWALLOWED");
                return default(TResult);
            }
        }

        /// <summary>
        /// Private helper method to encapsulate transactionscope and exception handling
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>        
        private static dynamic ExecuteInternal(Func<dynamic> code)
        {
            dynamic result = null;
            string callingMethod = string.Empty;
            bool nestedTiming = false;
            Stopwatch stopwatch = null;

            using (var section = TopLevelManagerContextSection.Start())
            {

                try
                {
                    //
                    // Check if debug tracing is turned on
                    //
                    if (Logger.IsDebugEnabled)
                    {
                        //
                        // If this is a nested call to Execute GetCallerForDebugTrace will
                        // return false because we don't want to trace this
                        //
                        if (GetCallerForDebugTrace(out callingMethod))
                        {
                            //
                            // Write debug trace
                            //
                            Logger.DebugFormat("Execute start: {0}", callingMethod);
                        }
                        else
                        {
                            nestedTiming = true;
                        }

                        //
                        // Initiate stopwatch for timing of this execution
                        //
                        stopwatch = new Stopwatch();
                        stopwatch.Start();
                    }

                    // Run the code
                    result = code();
                }
                catch (Exception e)
                {
                    //
                    // In debug mode we beep here
                    //
#if DEBUG
                    System.Media.SystemSounds.Exclamation.Play();
#endif

                    // Pass the exception to the ExecuteManager for further processing
                    ExecuteManager.Instance.ExceptionSwitch(e);
                }
                finally
                {
                    Exception rethrowException = null;

                    // If a stopwatch has been initiated log the elapsed execution time
                    if (stopwatch != null)
                    {
                        stopwatch.Stop();

                        if (nestedTiming)
                        {
                            Logger.DebugFormat("Nested execute done: {0} | {1} ms", callingMethod, stopwatch.ElapsedMilliseconds);
                        }
                        else
                        {
                            Logger.DebugFormat("Execute done: {0} | {1} ms", callingMethod, stopwatch.ElapsedMilliseconds);
                        }
                    }

                    if (rethrowException != null)
                    {
                        // Pass the exception to the ExecuteManager for further processing
                        ExecuteManager.Instance.ExceptionSwitch(rethrowException);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This method is called if debug trace is enabled and
        /// logs the name of the method that invoked ExecuteManager::Execute
        /// </summary>
        /// <returns></returns>
        private static bool GetCallerForDebugTrace(out string callingMethod)
        {
            callingMethod = string.Empty;

            // Get callstack
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();

            // Examine callstack
            int executeManagerExecuteOccurrenceCount = 0;

            foreach (StackFrame stackFrame in stackFrames)
            {
                //
                // Get method info from this place in the callstack
                //
                System.Reflection.MethodBase stackMethod = stackFrame.GetMethod();

                // If we found one occurrence of ExecuteManager::Execute and the variable
                // callingMethod isn't initiated we want to grab the method of the current
                // frame in the stack
                if ((executeManagerExecuteOccurrenceCount > 0) && (callingMethod.Equals(string.Empty)))
                {
                    if (!stackMethod.DeclaringType.FullName.Contains("Core.Facilitators.ExecuteManager")
                        &&
                        !stackMethod.DeclaringType.Name.StartsWith("BaseApi"))
                    {
                        var fullName = stackMethod.DeclaringType.GetFriendlyName();
                        callingMethod = string.Format("{0}::{1}", fullName, stackMethod.Name);
                    }
                }

                // Check for ExecuteManager::Execute
                if (((stackMethod.Name == "Execute") || (stackMethod.Name.StartsWith("ExecuteResumeOnError")))
                    && (stackMethod.DeclaringType == typeof(ExecuteManager)))
                {
                    executeManagerExecuteOccurrenceCount++;
                }
            }

            // We should only trace if ExecuteManager::Execute exists only once in the call stack
            return executeManagerExecuteOccurrenceCount == 1;
        }

        /// <summary>
        /// Eventhandler for the AppDomains UnhandledException event
        /// All unhandled exceptions will end up here and get logged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                // Extract the exception
                Exception exception = e.ExceptionObject as Exception;

                // Notify logger
                Logger.Error("Unhandled Exception", exception);
            }
            catch
            {
                // We don't want an exception to escape this eventhandler under any circumstances
                // as it most likely will crash the system 
            }
        }

        internal void ExceptionSwitch(System.Exception exception)
        {
            if (exception is AngOeException)
            {
                // Decendants of DomainException are "public" and should be rethrown to the caller
                // after first being logged
                Rethrow(exception);
            }
            else
            {
                // All other exception should be logged and "hidden" to the caller with an OperationFailedException
                ExecuteManager.Instance.ThrowOperationFailed(exception);
            }
        }

        /// <summary>
        /// Writes exception information to log and then throws the exception
        /// </summary>
        /// <param name="exception"></param>
        internal void Rethrow(Exception exception)
        {
            // Notify logger
            if (exception is AngOeException)
            {
                Logger.ErrorFormat("ZOSException: {0}", exception.GetType().FullName);
                Logger.Error(exception);
            }
            else
            {
                Logger.Error(exception);
            }

            // Throw the exception
            throw exception;
        }

        /// <summary>
        /// Writes exception information to log and then throws a general OperationFailed exception
        /// </summary>
        /// <param name="exception"></param>
        internal void ThrowOperationFailed(Exception exception)
        {
            // Notify logger
            Logger.Error(exception);

            throw new OperationFailedException();
        }


        private class TopLevelManagerContextSection : BaseCodeSection, IDisposable
        {
            protected override void OnStarted()
            {

            }

            protected override void OnNestedStarted(int level)
            {

            }

            protected override void OnEnded()
            {
                //ThreadContextManager.ClearThreadContext();
            }

            protected override void OnNestedEnded(int level)
            {

            }

            private TopLevelManagerContextSection()
            {
                BeginSection();
            }

            public static TopLevelManagerContextSection Start()
            {
                return new TopLevelManagerContextSection();
            }

            public static bool IsInSection
            {
                get
                {
                    return BaseCodeSection.IsThreadInSection<TopLevelManagerContextSection>();
                }
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}