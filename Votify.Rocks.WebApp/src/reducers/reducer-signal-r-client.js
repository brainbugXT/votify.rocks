import actionType from '../actions/action-types';

export default (state=null, action) => {

    switch(action.type){
        case actionType.signalRConnected:
            return action.signalRClientId;
        default:
            return state
    }
}