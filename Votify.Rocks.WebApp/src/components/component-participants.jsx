import React from 'react';
import Avatar from 'material-ui/Avatar';
import {List, ListItem} from 'material-ui/List';
import Divider from 'material-ui/Divider';

const voteColor = {
        0: 'silver',
        1: '#ff0000',
        2: '#f9c802',
        3: '#f9c802',
        4: '#a9d70b',
        5: '#a9d70b'
    };

const Participants = ({voteValue, participants}) => { 
    return (
        <List>
            {participants.map((participant) => { 
                let style = {
                    backgroundColor: voteValue ? voteColor[participant.voteValue] : 'silver',
                    };
                return  <div key={participant.uid}><ListItem  primaryText={participant.displayName}
                    rightAvatar={participant.voteValue ? <Avatar style={style}>{voteValue ? participant.voteValue : '?'}</Avatar> : null } />
                    <Divider />
                    </div>
            })}
        </List>
)};

export default Participants