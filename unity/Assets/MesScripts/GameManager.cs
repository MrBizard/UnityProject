using UnityEngine;

public class GameManager
{
    public static GameManager InstanceGame = new GameManager();
    public RealistPlayerController Player { get; private set; }

    public void SetPlayer(RealistPlayerController player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null");
            return;
        }

        Player = player;
        Debug.Log("Player set successfully!");
    }
}
