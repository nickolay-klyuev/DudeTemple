#if DEBUG
using Godot;
using Godot.Collections;
using System;

public static class CheckHelperStatic
{
    public static bool CheckScene(PackedScene scene, Node caller)
    {
        if (scene == null)
        {
            GD.PrintErr($"{caller.Name}: packed scene is missing!!!");
            return false;
        }

        return true;
    }

    public static bool CheckNode(Node node, Node caller)
    {
        if (node == null)
        {
            GD.PrintErr($"{caller.Name}: node is missing!!!");
            return false;
        }
        
        return true;
    }

    public static bool CheckNodes(Array<Node> nodes, Node caller)
    {
        bool bValid = true;
        foreach (Node node in nodes)
        {
            if (node == null)
            {
                GD.PrintErr($"{caller.Name}: node is missing!!!");
                bValid = false;
            }
        }

        return bValid;
    }

    public static bool CheckUI(Control ui, Node caller)
    {
        if (ui == null)
        {
            GD.PrintErr($"{caller.Name}: ui control is missing!!!");
            return false;
        }
        
        return true;
    }

    public static bool CheckUIs(Node caller, params Control[] uis)
    {
        bool bValid = true;

        foreach (Control ui in uis)
        {
            if (ui == null)
            {
                GD.PrintErr($"{caller.Name}: ui control is missing!!!");
                bValid = false;
            }
        }

        return bValid;
    }
}
#endif