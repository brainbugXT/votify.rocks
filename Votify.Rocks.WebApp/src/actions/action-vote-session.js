import * as actionCreators from './action-creators';
import * as api from '../services/service-vote-session';

const recomendedOnly = true;

export const SignalRConnected = (signalRClientId) => {
    return (dispatch, getState, args) => {
        dispatch(actionCreators.signalRConnected(signalRClientId));
    }
};

export const VoteSessionCreate = (participantName, description) => {
    return (dispatch, getState, args) => {
        var state = getState();
        const {signalRClient, session} = state;

        dispatch(actionCreators.createVoteSession(session.displayName, 'description'));
        const name = session.displayName.trim()
        const participant = {
            email: session.email.trim(),
            displayName: name.length ? name : session.randomName,
            isOrganizer: true,
            canVote: true
        };

        api.CreateVoteSession(participant, 'description', signalRClient)
        .then((votesession) => {
            dispatch(actionCreators.voteSessionCreated(votesession));
        })
        .catch((error) => {
            dispatch(actionCreators.apiError(error));
        });
    };
};

export const ParticipantJoinSignal = (participant) => {
    return (dispatch, getState, args) => {

        const {session} = getState();
/*
        if(participant.Uid === session.participantUid){
            return;
            //don't dispatch join event for own join signal
        }
*/
       dispatch(actionCreators.participantJoinSignal(participant));

       const {DisplayName} = participant;
       dispatch(actionCreators.applicationNotification(`${DisplayName} has arrived`));
    };
};

export const ParticipantLeaveSignal = (participantUid, displayName) => {
    return (dispatch, getState, args) => {

        const {session} = getState();
/*
        if(participantUid === session.participantUid){
            return;
            //don't dispatch leave event for own leave signal
        }
*/
       dispatch(actionCreators.participantLeaveSignal(participantUid));
       dispatch(actionCreators.applicationNotification(`${displayName} has left the session`));
    };
};

export const ParticipantVoteSignal = (participantUid, voteValue, voteSessionAverage) => {
    return (dispatch, getState, args) => {
       dispatch(actionCreators.participantVoteSignal(participantUid, voteValue, voteSessionAverage));
    };
};

export const JoinVoteSession = () => {
    return (dispatch, getState, args) => {
        var state = getState();
        const {signalRClient, session} = state;

       dispatch(actionCreators.joinVoteSession(session.key, session.displayName));
        const name = session.displayName.trim()
        const participant = {
            displayName: name.length ? name : session.randomName,
            isOrganizer: false,
            canVote: true
        };

        api.JoinVoteSession(session.key, participant, signalRClient).then((json) => {
            dispatch(actionCreators.voteSessionJoined(json.participantUid, json.votesession)); 
        })
        .catch((error) => {
            dispatch(actionCreators.apiError(error));
        });
    };
}

export const LeaveVoteSession = () => {
    return (dispatch, getState, args) => {
        var state = getState();
        const {signalRClient, session} = state;
        dispatch(actionCreators.leaveVoteSession(session.key, session.participantUid));

        api.LeaveVoteSession(session.key, session.participantUid, signalRClient).then(() => {
            dispatch(actionCreators.voteSessionLeft(session.participantUid));
        })
        .catch((error) => {
            dispatch(actionCreators.apiError(error));
        });
    };
}

export const DisplayNameChanged = (newName) => {
    return (dispatch, getState, args) => {
        dispatch(actionCreators.displayNameChanged(newName));
    };
}

export const EmailChanged = (newEmail) => {
    return (dispatch, getState, args) => {
        dispatch(actionCreators.emailChanged(newEmail));
    };
}

export const SessionKeyChanged = (newName) => {
    return (dispatch, getState, args) => {
        dispatch(actionCreators.sessionKeyChanged(newName));
    };
}

export const CastVote = (voteValue) => {
    return (dispatch, getState, args) => {
        var state = getState();
        const {session} = state;

        api.CastVote(session.key, session.participantUid, voteValue).then(() => {
            dispatch(actionCreators.castVote(session.key, session.participantUid, voteValue));
        })
        .catch((error) => {
            dispatch(actionCreators.apiError(error));
        });
    };
}

export const VoteSessionOpenSignal = () => {
    return (dispatch, getState, args) => {
        dispatch(actionCreators.voteSessionOpenSignal());
    }
}

export const DisplayApplicationNotification = (message) => {
    return (dispatch, getState, args) => {
        dispatch(actionCreators.applicationNotification(message));
    }
}

export const DismissNotification = () => {
    return (dispatch, getState, args) => {
        dispatch(actionCreators.notificationRequestClose());
    }
}

export const OpenVoteSession = () => {
    return (dispatch, getState, args) => {
        var state = getState();
        const {session} = state;

        dispatch(actionCreators.openVoteSession());
        api.OpenVoteSession(session.key, session.participantUid)
        .catch((error) => {
            dispatch(actionCreators.apiError(error));
        });
    }
}

export const GetRandomName = () => {
    return (dispatch, getState, args) => {
        api.RandomName().then(randomName => {
            dispatch(actionCreators.randomNameGenerated(randomName));
        });
    }
}
