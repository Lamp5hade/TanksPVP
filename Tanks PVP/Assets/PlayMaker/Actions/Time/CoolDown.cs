using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Time)]
	[Tooltip("Возвращает значение bool в зависимости от того, прошло ли заданное время CoolDown от начала отчета startCoolDown")]
	public class CoolDown : FsmStateAction
	{
        public enum TimeInfo
        {
            DeltaTime,
            TimeScale,
            SmoothDeltaTime,
            TimeInCurrentState,
            TimeSinceStartup,
            TimeSinceLevelLoad,
            RealTimeSinceStartup,
            RealTimeInCurrentState
        }

        public TimeInfo getInfo;

        [RequiredField]
        public FsmFloat CoolDownTime;
        public FsmEvent Passed;
        public FsmEvent Waiting;
        public FsmBool IsPassed;

        public bool everyFrame;

        private float startCoolDown;
        private float nowTime;

        public override void Reset()
        {
            getInfo = TimeInfo.TimeSinceLevelLoad;
            CoolDownTime = null;
            Passed = null;
            Waiting = null;
            IsPassed = null;
            nowTime = 0f;
          //  everyFrame = false;
        }

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if (CoolDownTime.Value <= 0)
            {
                Finish();
            }
            DoGetTimeInfo();

            if (nowTime - startCoolDown >= CoolDownTime.Value) IsPassed = true;

            if (IsPassed.Value == true)
            {
                startCoolDown = nowTime;
                IsPassed.Value = false;
                Fsm.Event(Passed);
            }
            else
            {
                Fsm.Event(Waiting);
            }
        }

        void DoGetTimeInfo()
        {
            switch (getInfo)
            {

                case TimeInfo.DeltaTime:
                    nowTime = Time.deltaTime;
                    break;

                case TimeInfo.TimeScale:
                    nowTime = Time.timeScale;
                    break;

                case TimeInfo.SmoothDeltaTime:
                    nowTime = Time.smoothDeltaTime;
                    break;

                case TimeInfo.TimeInCurrentState:
                    nowTime = State.StateTime;
                    break;

                case TimeInfo.TimeSinceStartup:
                    nowTime = Time.time;
                    break;

                case TimeInfo.TimeSinceLevelLoad:
                    nowTime = Time.timeSinceLevelLoad;
                    break;

                case TimeInfo.RealTimeSinceStartup:
                    nowTime = FsmTime.RealtimeSinceStartup;
                    break;

                case TimeInfo.RealTimeInCurrentState:
                    nowTime = FsmTime.RealtimeSinceStartup - State.RealStartTime;
                    break;

                default:
                    nowTime = 0f;
                    break;
            }
        }


    }

}
