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
                    "\nJ’aime commencer fort avec mes nouveaux collaborateurs, c’est comme un rite de passage." +
                    "Ma Comptable et moi-même avons besoin de travailler côte à côte, je veux que tu construises une structure qui relie nos bureaux. " +
                    "À toi de jouer !\"\t\t\t";
                break;
            case GameManager.Stage.Stage2:
                stageName.text = "FLOOR 2 : CAFETARIA";
                stageDescription.text = "Et voilà notre cafétéria ! Comme tu peux le voir, c’est un endroit très apprécié par nos employés. " +
                    "Cet espace informel a été pensé pour favoriser leur productivité : du café à volonté, pas de chaises pour éviter qu’ils ne s’installent et surtout, une corbeille de fruit frais toutes les semaines. " +
                    "Car un collaborateur heureux est un collaborateur efficace ! \r\nÇa me tient énormément à cœur tu sais qu’ils puissent se relaxer ensemble, ça améliore grandement l’aspect collaboratif. " +
                    "Bref, assez discuté ! Je t’invite à regarder ton prochain projet : on a une petite problématique de fuite… Comme tu peux le voir, il y a de la mousse partout et ça devient dangereux ! " +
                    "Je te laisse trouver une solution, ça serait dommage de fermer la cafétéria pour si peu.";
                break;
            case GameManager.Stage.Stage3:
                stageName.text = "FLOOR 3 : CO-WORKING FLOOR";
                stageDescription.text = "« Quel zèle dans la cafétéria ! Ça se sent que tu es fait pour WorkBuddies, tes solutions sont aussi disruptives que créatives ! Laisse moi te présenter notre espace de coworking : " +
                    "c’est ici qu’on teste toutes nos dernières trouvailles en terme d’organisation des employés et gestion de leur productivité. " +
                    "Comme tu peux le voir, on mesure aujourd’hui la régulation et la gouvernance des dynamiques intra-démographiques au sein d’un même bureau !" +
                    " …On dirait que ce n’est pas très parlant pour toi ? Ne t’en préoccupe pas, tu n’es qu’un Happiness Manager après tout ! " +
                    "Anyway, au travail ! J’ai une RH en arrêts travail répétés à virer et une remplaçante à lui trouver. Et toi, un espace à designer ! »";
                break;
            case GameManager.Stage.Stage4:
                stageName.text = "FLOOR 4 : TEAMBUILDING SPACE";
                stageDescription.text = "« Une équipe, c’est avant tout des gens soudés et capables de communiquer ! On ne peut malheureusement pas en dire autant de tout le monde… " +
                    "Mais que veux-tu, les absents ont toujours tort, it’s their loss comme on dit ! L’entreprise peut tourner sans eux… " +
                    "Des comptables et des employés, ça se recrute en un claquement de doigt ! Chez WorkBuddies on a besoin d’employés fidèles qui fit notre culture d’entreprise, pas de paresseux. " +
                    "Assez discutaillé, c’est une mission de crise que je te confie aujourd’hui. Avec les employés restants, j’aimerais que tu mettes au goût des pratiques actuelles notre espace de team building ! " +
                    "Avec ces pertes récentes, c’est important qu’ils se sentent valorisés et irremplaçables. »";
                break;
            case GameManager.Stage.Stage5:
                stageName.text = "FLOOR 5 : INTERNS OFFICE";
                stageDescription.text = "C’est la cata ! Entre les arrêts maladies à rallonge, les simples démissions et les soit disant « burn out », cette grande maladie imaginaire du 21ème siècle ! Cette entreprise est en train de couler ! " +
                    "C’est comme ça qu’on me remercie ! « Pousse-les à être meilleurs » qu’on m’a dit, « tu verras tu seras récompensé » Tu parles ! Travailler ça n’intéresse plus personne, la satisfaction de l’effort est mort et cette entreprise avec ! " +
                    "Je suis terriblement déçu, c’est une trahison sans nom ! Dans ces conditions tu comprendras que je ne peux pas continuer ta période d’essai, nous allons devoir nous séparer ici… " +
                    "Hop hop hop, pas si vite ! Avant que tu fasses tes affaires, j’ai une dernière tâche à te confier. On a plus vraiment de budget mais je compte sur toi pour refaire à neuf le cagib- je veux dire le bureau des stagiaires !";
                break;
            default:
                stageName.text = "STAGE ??? : UNKNOWN STAGE ???";
                stageDescription.text = "Unknown Description.";
                break;
        }
    }
}
