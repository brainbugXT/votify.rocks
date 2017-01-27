import actionType from '../actions/action-types';

const initialState = {
  sessionKey: null,
  organizerUid: null,
  participants: [],
  voteAverage: 0,
  openForVoting: false,
  description: ""
};

export default (state=initialState, action) => {
    let newParticipantList = [];
    switch(action.type){
        case actionType.participantJoinSignal:
            const {participants} = state;
            const newParticipant = {
                uid: action.participant.Uid,
                displayName: action.participant.DisplayName,
                isOrganizer: action.participant.IsOrganizer,
                voteValue: action.participant.VoteValue,
                canVote: action.participant.CanVote
            };
            return {...state, participants: [...participants, newParticipant]};
        case actionType.participantLeaveSignal:
            newParticipantList = state.participants.filter(participant => {
                return participant.uid !== action.participantUid
            });
            return {...state, participants: newParticipantList};
        case actionType.voteSessionLeft:
            return initialState;
        case actionType.voteSessionJoined:
            return Object.assign({}, state, action.voteSession);
        case actionType.voteSessionCreated:
            return Object.assign({}, state, action.voteSession);
        case actionType.participantVoteSignal:
            newParticipantList = state.participants.map((participant) => {
                if(participant.uid === action.participantUid){
                    return {...participant, voteValue: action.voteValue};
                }
                return participant;
            });
            return {...state, voteAverage: action.voteSessionAverage, participants: newParticipantList};
        case actionType.voteSessionOpenSignal:
            return {...state, openForVoting: true}
        default:
            return state
    }
}