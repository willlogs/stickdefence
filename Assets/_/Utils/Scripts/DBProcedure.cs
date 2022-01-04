using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.Utils
{
    [Serializable]
    public class ProcedureStep
    {
        public UnityEvent OnActivation;
        public event Action OnDone;

        public void Activate()
        {
            OnActivation?.Invoke();
            OnDone?.Invoke();
        }
    }

    public class DBProcedure : MonoBehaviour
    {
        public ProcedureStep[] steps;
        public ProcedureStep curStep;
        public bool hasStep;

        public void Cue()
        {
            curStep.Activate();
        }

        private int index;

        private void GoToNext()
        {
            curStep.OnDone -= GoToNext;
            hasStep = index < steps.Length;
            if (hasStep)
            {
                curStep = steps[index];
                curStep.OnDone += GoToNext;
                index++;
            }
        }

        private void Start()
        {
            hasStep = steps.Length > 0;

            if (hasStep)
            {
                index = 1;
                curStep = steps[0];
                curStep.OnDone += GoToNext;
            }
        }
    }
}