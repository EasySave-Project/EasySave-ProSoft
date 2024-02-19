namespace EasySave.model
{
    public interface IStrategieSave
    {

        void SauvegardeState(State etat, string cheminDossier);
    }
}