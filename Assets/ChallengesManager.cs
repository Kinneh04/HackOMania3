using System.Collections.Generic;
using UnityEngine;

public class ChallengesManager : MonoBehaviour
{

    [Header("Variables")]
    public List<Challenge> Challenges = new();

    [Header("Query")]
    public List<string> strings = new();
    public List<Challenge> QueriedChallenges = new();

    public List<Challenge> FindChallengesWithContext(List<string> Context)
    {
        List<Challenge> matchedChallenges = new List<Challenge>();

        // Iterate through all challenges
        foreach (var challenge in Challenges)
        {
            // Check if any keyword token is in the context list
            foreach (var keyword in challenge.KeywordTokens)
            {
                if (Context.Contains(keyword))
                {
                    matchedChallenges.Add(challenge);
                    break; // Break to avoid adding the same challenge multiple times
                }
            }
        }

        return matchedChallenges;
    }

    public void ReturnMatchedChallenges(string response)
    {
        response = response.ToLower();
        QueriedChallenges.Clear();
        strings.Clear();
        foreach (Challenge C in Challenges)
        {
            foreach (string s in C.KeywordTokens)
            {
                string ss = s.ToLower();
                
                if (response.Contains(ss))
                {
                    strings.Add(s);
                    QueriedChallenges.Add(C);
                    break;
                }
            }
        }
    }
}
