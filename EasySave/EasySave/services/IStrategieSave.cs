using EasySave.model;

namespace EasySave.services
{
    public interface IStrategieSave
    {

        void SaveState(State state);

        void SaveLog(Log log);
    }
}