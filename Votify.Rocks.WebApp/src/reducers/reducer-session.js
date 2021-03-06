import cookie from 'react-cookie';
import actionType from '../actions/action-types';
import {userCookieName} from '../globals'

const initialState = {
    key: '',
    participantUid: null,
    displayName: cookie.load(userCookieName),
    email: '',
    voteValue: 0,
    randomName: '',
    canVote: true,
    description: '',
};

export default (state=initialState, action) => {

    switch(action.type){
        case actionType.sessionKeyChanged:
            return {...state, key: action.newKey};
        case actionType.displayNameChanged:
            return {...state, displayName: action.newName};
        case actionType.emailChanged:
            return {...state, email: action.newEmail};
        case actionType.descriptionChanged:
            return {...state, description: action.newDescription};
        case actionType.canVoteChanged:
            return {...state, canVote: action.newCanVoteValue};
        case actionType.voteSessionJoined:
            return {...state, participantUid: action.participantUid}
        case actionType.voteSessionCreated:
            return {...state, participantUid: action.voteSession.organizerUid, key: action.voteSession.sessionKey};
        case actionType.voteSessionLeft:
            return {...initialState, randomName: state.randomName};
        case actionType.castVote:
            return {...state, voteValue: action.voteValue}
        case actionType.randomNameGenerated:
            return {...state, randomName: action.randomName}
        default:
            return state
    }
}