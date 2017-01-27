import {combineReducers} from 'redux';
import VoteSessionReducer from './reducer-vote-session';
import SignalRClientReducer from './reducer-signal-r-client';
import SessionReducer from './reducer-session';
import NotificationReducer from './reducer-notification';

const reducers = combineReducers({
        voteSession: VoteSessionReducer,
        signalRClient: SignalRClientReducer,
        session: SessionReducer,
        notification: NotificationReducer
    }
);

export default reducers;
