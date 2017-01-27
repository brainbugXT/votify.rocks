import actionType from './action-types';

export const createVoteSession = (participant, description) => ({
        type: actionType.createVoteSession,
        participant: participant,
        description: description
});

export const voteSessionCreated = (voteSession) => ({
        type: actionType.voteSessionCreated,
        voteSession: voteSession
});

export const participantJoinSignal   = (participant) => ({
        type: actionType.participantJoinSignal,
        participant: participant
});

export const participantLeaveSignal = (participantUid) => ({
        type: actionType.participantLeaveSignal,
        participantUid: participantUid
});

export const participantVoteSignal = (participantUid, voteValue, voteSessionAverage) => ({
        type: actionType.participantVoteSignal,
        participantUid: participantUid,
        voteValue: voteValue,
        voteSessionAverage: voteSessionAverage
});

export const signalRConnected = (signalRClientId) => ({
        type: actionType.signalRConnected,
        signalRClientId: signalRClientId
});

export const joinVoteSession = (voteSessionKey, participantName) => ({
        type: actionType.joinVoteSession,
        voteSessionKey: voteSessionKey,
        participantName: participantName
});

export const voteSessionJoined = (participantUid, voteSession) => ({
        type: actionType.voteSessionJoined,
        participantUid: participantUid,
        voteSession: voteSession
});

export const leaveVoteSession = (voteSessionKey, participantUid) => ({
        type: actionType.leaveVoteSession,
        voteSessionKey: voteSessionKey,
        participantUid: participantUid
});

export const voteSessionLeft = (participantUid) => ({
        type: actionType.voteSessionLeft,
        participantUid: participantUid
});

export const displayNameChanged = (newName) => ({
        type: actionType.displayNameChanged,
        newName: newName
});

export const sessionKeyChanged = (newKey) => ({
        type: actionType.sessionKeyChanged,
        newKey: newKey
});

export const castVote = (voteSessionKey, participantUid, voteValue) => ({
        type: actionType.castVote,
        voteSessionKey: voteSessionKey,
        participantUid: participantUid,
        voteValue: voteValue
});

export const voteCast = () => ({
        type: actionType.voteCast,
});

export const apiError = (error) => ({
        type: actionType.apiError,
        error: error
});

export const voteSessionOpenSignal = () => ({
        type: actionType.voteSessionOpenSignal
})

export const openVoteSession = () => ({
        type: actionType.openVoteSession
})

export const notificationRequestClose = () => ({
        type: actionType.dismissNotification
})

export const applicationNotification = (message) => ({
        type: actionType.applicationNotification,
        message: message
})

export const randomNameGenerated = (randomName) => ({
        type: actionType.randomNameGenerated,
        randomName: randomName
})