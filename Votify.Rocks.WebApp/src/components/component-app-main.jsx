import _globals from '../globals';
import React, {createClass} from 'react';
import {connect} from 'react-redux';
import * as actions from '../actions/action-vote-session';
import SessionAccess from './component-session-access';
import VoteSession from './component-vote-session';
import Snackbar from 'material-ui/Snackbar';
//import $ from 'jquery'
import 'ms-signalr-client'

require('!style!css!sass!../scss/style.scss');

const  VoteApp = createClass ({
    componentDidMount () {
        let connection = $.hubConnection(_globals.apiUrl);
        let proxy = connection.createHubProxy('simpleVoteSessionHub');
        
            proxy.on('participantJoin', participant => {
                this.props.onParticipantJoinSignal(participant);
            });

            proxy.on('participantLeave', (participantUid, displayName) => {
                this.props.onParticipantLeaveSignal(participantUid, displayName);
            });

            proxy.on('participantVote', (participantUid, voteValue, voteSessionAverage) => {
                this.props.onParticipantVoteSignal(participantUid, voteValue, voteSessionAverage);
            });

            proxy.on('voteSessionOpen', () => {
                this.props.onVoteSessionOpenSignal();
            });

            proxy.on('ping', msg => {
                console.log('ping',msg);
                 this.props.onDisplayApplicationNotification(msg);
            });
            
            // atempt connection, and handle errors
            connection.start({ jsonp: true })
            .done(() => { 
                this.props.onSignalRConnected(connection.id);
                //console.log('Now connected, connection ID=' + connection.id);
            })
            .fail(() => {
                this.props.onDisplayApplicationNotification('Could not connect SignalR');
                console.log('Could not connect SignalR'); 
            });

            this.props.onGenerateRandomName();
        },

        handleOnCreateClick () {
            this.props.onCreateVoteSession();
        },

        handleOnJoinClick () {
            this.props.onJoinVoteSession();
        },

        handleOnLeaveClick () {
            this.props.onLeaveVoteSession();
        },

        handleOnVoteCast (voteValue) {
            this.props.onVoteCast(voteValue);
        },

        handleOnSessionOpenClick () {
            this.props.onSessionOpen();
        },

        handleSessionKeyChange (event) {
            this.props.onVoteSessionKeyChange(event.target.value);
        },

        handleDisplayNameChange(event) {
            this.props.onDisplayNameChange(event.target.value);
        },

        handleNotificationRequestToClose() {
            this.props.onNotificationRequestToClose();
        },

        render () {
            return (
            <div>
                <SessionAccess session={this.props.session} onSessionKeyChange={this.handleSessionKeyChange} onDisplayNameChange={this.handleDisplayNameChange} onCreateClick={this.handleOnCreateClick} onJoinClick={this.handleOnJoinClick} />
                <VoteSession session={this.props.session} voteSession={this.props.voteSession} onVoteCast={this.handleOnVoteCast} onLeaveSession={this.handleOnLeaveClick} onOpenSession={this.handleOnSessionOpenClick}/>
                <Snackbar open={this.props.notification.open} message={this.props.notification.message} onRequestClose={this.handleNotificationRequestToClose} autoHideDuration={2000} />
            </div>)
        }
  });

const mapStateToProps = ({session, voteSession, notification}) => ({
    voteSession,
    session,
    notification
});

const mapDispatchToProps = (dispatch, ownProps) => ({
    onSignalRConnected (signalRClientId) {
        dispatch(actions.SignalRConnected(signalRClientId))
    },
    onCreateVoteSession (participant, description) {
        dispatch(actions.VoteSessionCreate(participant, description));
    },
    onJoinVoteSession (voteSessionKey, participantName) {
        dispatch(actions.JoinVoteSession(voteSessionKey, participantName));
    },
    onLeaveVoteSession () {
        dispatch(actions.LeaveVoteSession());
    },
    onParticipantJoinSignal(participant) {
        dispatch(actions.ParticipantJoinSignal(participant));
    },
    onParticipantLeaveSignal(participantUid, displayName) {
        dispatch(actions.ParticipantLeaveSignal(participantUid, displayName));
    },
    onParticipantVoteSignal(participantUid, voteValue, voteSessionAverage) {
        dispatch(actions.ParticipantVoteSignal(participantUid, voteValue, voteSessionAverage));
    },
    onVoteCast(voteValue){
        dispatch(actions.CastVote(voteValue));
    },
    onSessionOpen () {
        dispatch(actions.OpenVoteSession());
    },
    onVoteSessionKeyChange (newKey) {
        dispatch(actions.SessionKeyChanged(newKey));
    },
    onDisplayNameChange (newName) {
        dispatch(actions.DisplayNameChanged(newName));
    },
    onVoteSessionOpenSignal () {
        dispatch(actions.VoteSessionOpenSignal());
    },
    onNotificationRequestToClose() {
        dispatch(actions.DismissNotification());
    },
    onDisplayApplicationNotification(message) {
        dispatch(actions.DisplayApplicationNotification(message));
    },
    onGenerateRandomName() {
        dispatch(actions.GetRandomName());
    }
});

export default connect(mapStateToProps, mapDispatchToProps)(VoteApp);