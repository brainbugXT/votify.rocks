import React from 'react';
import Paper from 'material-ui/Paper';
import RaisedButton from 'material-ui/RaisedButton';
import LockIcon from 'material-ui/svg-icons/action/lock'

const btnStyle = {
        height: 50,
        width: 50,
        margin: 5,
        padding: 10,
        textAlign: 'center',
        display: 'inline-block',
        cursor: 'not-allowed',
        backgroundColor: 'gainsboro',
        fontWeight: 'bold',
        fontSize: '26px',
        color: 'white',
};

const btnIconStyle = {
    marginTop: 2,
}

const buttons = [
        {no: 1, color: '#ff0000'},
        {no: 2, color: '#f9c802'},
        {no: 3, color: '#f9c802'},
        {no: 4, color: '#a9d70b'},
        {no: 5, color: '#a9d70b'}
    ];

const VoteButtons = ({voteValue, voteSessionOpen, onVoteCast}) => {
    return (
        <div>
            {
                buttons.map(button => {
                    let buttonStyle = {...btnStyle, backgroundColor: button.color, cursor: 'pointer' };
                    return voteSessionOpen && (voteValue === button.no || voteValue === 0)
                    ? <Paper key={button.no} onTouchTap={() => onVoteCast(button.no)} style={buttonStyle} zDepth={1} circle={true} >{button.no}</Paper>
                    : <Paper key={button.no} style={btnStyle} zDepth={1} circle={true} ><LockIcon color={'silver'} style={btnIconStyle}/></Paper>
                })
            }
        </div>)
};

export default VoteButtons