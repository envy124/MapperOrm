﻿using System;
using System.Threading;

namespace MapperOrm.Repository
{
    public static class Session
    {
        private static readonly ThreadLocal<IUnitOfWork> CurrentThreadData = new ThreadLocal<IUnitOfWork>(true);
  
        public static IUnitOfWork Current
        {
            get { return CurrentThreadData.Value; }
            private set { CurrentThreadData.Value = value; }
        }

        public static IUnitOfWork Create(IUnitOfWork uow)
        {
            return Current ?? (Current = uow);
        }
        internal static IObjectTracker GetObjectTracker(string connectionName)
        {
            var uow = Current;
            if (uow == null)
            {
                throw new ApplicationException("Current context of unit of work is did't make. Create unit of work context and using UnitPfWorkManager.");
            }

            var detector = uow as IDetector;
            if (detector == null)
            {
                throw new ApplicationException("Current context of unit of work is did't make. Create unit of work context and using UnitPfWorkManager.");
            }

            return detector.ObjectDetector[connectionName];
        }

        internal static void OnDisponse(object sender, EventArgs e)
        {
            CurrentThreadData.Values.Remove(CurrentThreadData.Value);
            CurrentThreadData.Value = null;
        }
    }
}
