using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourTree
{
    //Class to split the ui interface between the BehaviourTree section (with the nodes)
    //and an inspector section (with the values of the node selected)
    public class SplitView : TwoPaneSplitView
    {
      public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}
