using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net
{
    /// <summary>
    /// The networked gameplay interface which holds stubs for methods which the networking system will call.
    /// </summary>
    public interface vxINetGameplayScene
    {
        /// <summary>
        /// Called when a player joins
        /// </summary>
        void OnNetPlayerJoined(vxNetPlayerInfo playerInfo);

        /// <summary>
        /// Called when a player disconnects from the match
        /// </summary>
        void OnNetPlayerDisconnected(vxNetPlayerInfo playerInfo);

        /// <summary>
        /// Called when a players status is updated, such as 'Waiting In Lobby' to 'Ready to play'
        /// </summary>
        void OnNetPlayerStatusUpdated(vxNetPlayerInfo playerInfo);

        /// <summary>
        /// Called when a player's entity state has changed, such as updating position
        /// </summary>
        /// <param name="stateUpdate">the player entity state</param>
        /// <param name="TimeDelay">The time delay</param>
        void OnNetPlayerEntityStateUpdated(vxNetmsgUpdatePlayerEntityState stateUpdate, float TimeDelay);

        /// <summary>
        /// Called when the net session state has changed, such as being told by the server that we're going from lobby to playing the game
        /// </summary>
        /// <param name="oldSessionState">The old net sesison state</param>
        /// <param name="newSessionState">The new net session state we're transitioning to</param>
        /// <param name="TimeDelay">The time delay</param>
        void OnNetSessionStateChanged(vxEnumNetSessionState oldSessionState, vxEnumNetSessionState newSessionState, float TimeDelay);

        /// <summary>
        /// Called when a level event occurs such as an item is spawned, missile hits charatcer, player scores etc...
        /// </summary>
        /// <param name="lvlEvent">The level event</param>
        void OnNetLevelEvent(vxNetmsgLevelEvent lvlEvent);

        /// <summary>
        /// The server has shutdown for some reason
        /// </summary>
        void OnNetServerDisconnected(vxNetDisconnectedEventReason reason);
    }
}
