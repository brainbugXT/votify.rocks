import {combineReducers} from 'redux';
import NotificationReducer from './reducer-notification'
import SessionReducer from './reducer-session'
import SignalRClientReducer from './reducer-signal-r-client'
import VoteSessionReducer from './reducer-vote-session'

const reducers = combineReducers({
        notification: NotificationReducer,
        session: SessionReducer,
        signalRClient: SignalRClientReducer,
        voteSession: VoteSessionReducer,
    }
);

export default reducers;