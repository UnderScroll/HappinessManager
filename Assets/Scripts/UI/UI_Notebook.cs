using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Notebook : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stageName;
    [SerializeField] TextMeshProUGUI stageDescription;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager != null)
        {
            InitLevelName();
        }
    }
    private void InitLevelName()
    {
        switch (_gameManager.CurrentStage)
        {
            case GameManager.Stage.Stage1:
                stageName.text = "FLOOR 1 : BOSS WORKSPACE";
                stageDescription.text = "\"Bienvenue !\r" +
                    "\nJ�aime commencer fort avec mes nouveaux collaborateurs, c�est comme un rite de passage." +
                    "Ma Comptable et moi-m�me avons besoin de travailler c�te � c�te, je veux que tu construises une structure qui relie nos bureaux. " +
                    "� toi de jouer !\"\t\t\t";
                break;
            case GameManager.Stage.Stage2:
                stageName.text = "FLOOR 2 : CAFETARIA";
                stageDescription.text = "Et voil� notre caf�t�ria ! Comme tu peux le voir, c�est un endroit tr�s appr�ci� par nos employ�s. " +
                    "Cet espace informel a �t� pens� pour favoriser leur productivit� : du caf� � volont�, pas de chaises pour �viter qu�ils ne s�installent et surtout, une corbeille de fruit frais toutes les semaines. " +
                    "Car un collaborateur heureux est un collaborateur efficace ! \r\n�a me tient �norm�ment � c�ur tu sais qu�ils puissent se relaxer ensemble, �a am�liore grandement l�aspect collaboratif. " +
                    "Bref, assez discut� ! Je t�invite � regarder ton prochain projet : on a une petite probl�matique de fuite� Comme tu peux le voir, il y a de la mousse partout et �a devient dangereux ! " +
                    "Je te laisse trouver une solution, �a serait dommage de fermer la caf�t�ria pour si peu.";
                break;
            case GameManager.Stage.Stage3:
                stageName.text = "FLOOR 3 : CO-WORKING FLOOR";
                stageDescription.text = "��Quel z�le dans la caf�t�ria ! �a se sent que tu es fait pour WorkBuddies, tes solutions sont aussi disruptives que cr�atives ! Laisse moi te pr�senter notre espace de coworking : " +
                    "c�est ici qu�on teste toutes nos derni�res trouvailles en terme d�organisation des employ�s et gestion de leur productivit�. " +
                    "Comme tu peux le voir, on mesure aujourd�hui la r�gulation et la gouvernance des dynamiques intra-d�mographiques au sein d�un m�me bureau !" +
                    " �On dirait que ce n�est pas tr�s parlant pour toi ? Ne t�en pr�occupe pas, tu n�es qu�un Happiness Manager apr�s tout ! " +
                    "Anyway, au travail ! J�ai une RH en arr�ts travail r�p�t�s � virer et une rempla�ante � lui trouver. Et toi, un espace � designer ! �";
                break;
            case GameManager.Stage.Stage4:
                stageName.text = "FLOOR 4 : TEAMBUILDING SPACE";
                stageDescription.text = "��Une �quipe, c�est avant tout des gens soud�s et capables de communiquer ! On ne peut malheureusement pas en dire autant de tout le monde� " +
                    "Mais que veux-tu, les absents ont toujours tort, it�s their loss comme on dit ! L�entreprise peut tourner sans eux� " +
                    "Des comptables et des employ�s, �a se recrute en un claquement de doigt ! Chez WorkBuddies on a besoin d�employ�s fid�les qui fit notre culture d�entreprise, pas de paresseux. " +
                    "Assez discutaill�, c�est une mission de crise que je te confie aujourd�hui. Avec les employ�s restants, j�aimerais que tu mettes au go�t des pratiques actuelles notre espace de team building ! " +
                    "Avec ces pertes r�centes, c�est important qu�ils se sentent valoris�s et irrempla�ables.��";
                break;
            case GameManager.Stage.Stage5:
                stageName.text = "FLOOR 5 : INTERNS OFFICE";
                stageDescription.text = "C�est la cata ! Entre les arr�ts maladies � rallonge, les simples d�missions et les soit disant ��burn out��, cette grande maladie imaginaire du 21�me si�cle ! Cette entreprise est en train de couler ! " +
                    "C�est comme �a qu�on me remercie ! ��Pousse-les � �tre meilleurs�� qu�on m�a dit, ��tu verras tu seras r�compens頻 Tu parles ! Travailler �a n�int�resse plus personne, la satisfaction de l�effort est mort et cette entreprise avec ! " +
                    "Je suis terriblement d��u, c�est une trahison sans nom ! Dans ces conditions tu comprendras que je ne peux pas continuer ta p�riode d�essai, nous allons devoir nous s�parer ici� " +
                    "Hop hop hop, pas si vite ! Avant que tu fasses tes affaires, j�ai une derni�re t�che � te confier. On a plus vraiment de budget mais je compte sur toi pour refaire � neuf le cagib- je veux dire le bureau des stagiaires !";
                break;
            default:
                stageName.text = "STAGE ??? : UNKNOWN STAGE ???";
                stageDescription.text = "Unknown Description.";
                break;
        }
    }
}
