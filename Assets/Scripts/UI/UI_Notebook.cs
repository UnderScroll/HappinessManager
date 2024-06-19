using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Notebook : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stageName;
    [SerializeField] TextMeshProUGUI stageDescription;
    [SerializeField] List<GameObject> infoPanels;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager != null)
        {
            Init();
        }
    }
    private void Init()
    {
        switch (_gameManager.CurrentStage)
        {
            case GameManager.Stage.Stage1:
                stageName.text = "FLOOR 1 : BOSS WORKSPACE";
                stageDescription.text = "\"« Welcome!!\r" +
                    "\nI like to hit the ground running with my new team members; it's akin to an initiation into the corporate culture" +
                    "My accountant and I need to work side by side, so I want you to build a structure to link our desks. »" +
                    "Time for you to shine!\"\t\t\t";
                if (infoPanels[0] != null)
                    infoPanels[0].SetActive(true);
                break;
            case GameManager.Stage.Stage2:
                stageName.text = "FLOOR 2 : CAFETARIA";
                stageDescription.text = "« And here's our cafeteria! As you can see, it's a highly valued space by our employees. " +
                    "This informal area has been designed to enhance their productivity: unlimited coffee, no chairs to prevent them from settling in, and most importantly, a fresh fruit basket every week. " +
                    "Because a happy employee is an efficient one! It means a lot to me that they can relax together; it greatly enhances collaboration. " +
                    "Anyway, enough talking! I invite you to look into your next project: we have a small leakage issue... As you can see, there's foam everywhere and it's becoming hazardous! " +
                    "I'll let you find a solution; it would be a shame to close the cafeteria for such a trivial matter. »";
                if (infoPanels[1] != null)
                    infoPanels[1].SetActive(true);
                break;
            case GameManager.Stage.Stage3:
                stageName.text = "FLOOR 3 : CO-WORKING FLOOR";
                stageDescription.text = "« What a dynamo in the cafeteria! It's clear that you're made for WorkBuddies; your solutions are as disruptive as they are creative! Let me introduce you to our coworking space: " +
                    "this is where we evaluate all our latest innovations in employee organization and productivity management. " +
                    "As you can see, today we are measuring the regulation and governance of intra-demographic dynamics within a single office! " +
                    "...It looks like that's not very clear to you? Don't worry about it, you're just a Happiness Manager after all! " +
                    "Anyway, let's get to work! I have an HR person on repeated sick leave to fire and a replacement to find for her. And you have a space to design! »";
                if (infoPanels[2] != null)
                    infoPanels[2].SetActive(true);
                break;
            case GameManager.Stage.Stage4:
                stageName.text = "FLOOR 4 : TEAMBUILDING SPACE";
                stageDescription.text = "« A team, it's primarily about people who are united and capable of communicating! Unfortunately, we can't say the same for everyone... " +
                    "But hey, as they say, the absents are always wrong, it’s their loss! The company can function without them... " +
                    "Accountants and employees, they can be recruited in the blink of an eye! At WorkBuddies, we need loyal employees who embody our company culture, not lazy ones. " +
                    "Enough chatter, today I'm entrusting you with a crisis mission. With the remaining employees, I would like you to modernize our team building space to align with current practices! " +
                    "Given these recent losses, it's important that they feel valued and indispensable. »";
                if (infoPanels[3] != null)
                    infoPanels[3].SetActive(true);
                break;
            case GameManager.Stage.Stage5:
                stageName.text = "FLOOR 5 : INTERNS OFFICE";
                stageDescription.text = "« It’s a total mess! With the endless sick leaves, the simple resignations, and these so-called ‘burnouts’—the trendy imaginary disease of the 21st century! This company is sinking! " +
                    "And this is how they show their appreciation? ‘Push them to excel,’ they said ‘you’ll be rewarded.’ Yeah, right! Nobody cares about hard work anymore; the gratification of effort has become obsolete, and so has this company! " +
                    "I’m terribly disappointed: it’s a betrayal beyond words. Given the circumstances, you’ll understand that I can’t extend your probationary period; we’ll have to part ways here… " +
                    "Hold on a second! Before you pack your things, I have one last task for you. We don't really have a budget anymore, but I'm counting on you to refurbish the storage room— I mean the intern's office! »";
                if (infoPanels[4] != null)
                    infoPanels[4].SetActive(true);
                break;
            default:
                stageName.text = "STAGE ??? : UNKNOWN STAGE ???";
                stageDescription.text = "Unknown Description.";
                break;
        }
    }
}
