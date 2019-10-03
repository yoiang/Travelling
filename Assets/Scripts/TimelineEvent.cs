using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

class TimelineEvent : IPlayableAsset
{
    public double duration => throw new System.NotImplementedException();

    public IEnumerable<PlayableBinding> outputs => throw new System.NotImplementedException();

    public Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        throw new System.NotImplementedException();
    }
}