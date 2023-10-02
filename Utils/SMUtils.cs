using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Utils
{
    public class SMUtils
    {
        public static T GetStateMachineCurrect<T>(IStateMachine stateMachine) where T:StateMachine
        {
            return stateMachine as T;
        }
    }
}
