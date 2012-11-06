using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using NLog;
using Raven.Abstractions.Exceptions;
using System.Web.Hosting;

namespace Integer.Infrastructure.Tasks
{
    public abstract class BackgroundTask : IRegisteredObject
    {
        protected IDocumentSession DocumentSession;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly object shutDownLock = new object();
        private bool shuttingDown;

        public BackgroundTask()
        {
            HostingEnvironment.RegisterObject(this);
        }

        protected virtual void Initialize(IDocumentSession session)
        {
            DocumentSession = session;
            DocumentSession.Advanced.UseOptimisticConcurrency = true;
        }

        protected virtual void OnError(Exception e)
        {            
        }

        public bool? Run(IDocumentSession openSession)
        {
            lock (shutDownLock)
            {
                if (shuttingDown)
                {
                    return false;
                }
                return Work(openSession);
            }            
        }

        private bool? Work(IDocumentSession openSession) 
        {
            Initialize(openSession);
            try
            {
                Execute();
                DocumentSession.SaveChanges();
                TaskExecutor.StartExecuting();
                return true;
            }
            catch (ConcurrencyException e)
            {
                logger.ErrorException("Could not execute task " + GetType().Name, e);
                OnError(e);
                return null;
            }
            catch (Exception e)
            {
                logger.ErrorException("Could not execute task " + GetType().Name, e);
                OnError(e);
                return false;
            }
            finally
            {
                TaskExecutor.Discard();
            }
        }

        public abstract void Execute();

        public void Stop(bool immediate)
        {
            lock (shutDownLock)
            {
                shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this); 
        }
    }
}
