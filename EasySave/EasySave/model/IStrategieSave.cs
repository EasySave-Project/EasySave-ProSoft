namespace EasySave.model
{
    public interface IStrategieSave
    {

        void SauvegardeState(State etat, string cheminDossier);

        void SaveLog(Log log);
    }
}