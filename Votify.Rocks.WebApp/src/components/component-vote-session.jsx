import React from 'react';
import Participants from './component-participants';
import Gage from './component-gage';
import VoteButtons from './component-vote-buttons';
import AppBar from 'material-ui/AppBar';
import IconButton from 'material-ui/IconButton';
import IconMenu from 'material-ui/IconMenu';
import MailIcon from 'material-ui/svg-icons/content/mail';
import MenuItem from 'material-ui/MenuItem';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import NavigationCloseIcon from 'material-ui/svg-icons/navigation/close';
import ExitAppIcon from 'material-ui/svg-icons/action/exit-to-app'
import LockOpenIcon from 'material-ui/svg-icons/action/lock-open'
import FloatingActionButton from 'material-ui/FloatingActionButton';

const VoteSession = ({session, voteSession, onVoteCast, onLeaveSession, onOpenSession, notification}) => {

    const isOrganizer = session.participantUid === voteSession.organizerUid
    if(voteSession && voteSession.sessionKey && voteSession.sessionKey.length){
        return (
            <div className="vote-session-container">
                <div className="app-bar">
                    <AppBar
                        title={session.displayName}
                        iconElementLeft={
                            <IconButton>{voteSession.voteAverage ? <MailIcon /> : null }</IconButton>
                        }
                        iconElementRight={
                            <IconButton onTouchTap={onLeaveSession}><ExitAppIcon /></IconButton>
                        }
                    />
                </div>
                <div className="session-components">
                    <Gage title={voteSession.sessionKey} value={voteSession.voteAverage} voteValue={session.voteValue}/>
                    <VoteButtons voteValue={session.voteValue} voteSessionOpen={voteSession.openForVoting} onVoteCast={onVoteCast} />
                    <div className="participant-list">
                        <Participants  participants={voteSession.participants} voteValue={session.voteValue} />
                    </div>
                </div>
                    {
                        !voteSession.openForVoting && isOrganizer
                            ? <FloatingActionButton className="floating-start-session-button" onTouchTap={onOpenSession} >
                                <LockOpenIcon />
                            </FloatingActionButton> 
                            : null
                    }
            </div>);
    }

    return null;
};

export default VoteSession