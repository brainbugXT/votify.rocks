import actionType from '../actions/action-types';

const initialState = {
    open: false,
    message: ''
}

export default (state=initialState, action) => {

    switch(action.type){
        case actionType.applicationNotification:
            return {open: true, message: action.message}
        case actionType.apiError:
            return {open: true, message: action.error};
        case actionType.dismissNotification:
            return initialState;
        default:
            return state
    }
}