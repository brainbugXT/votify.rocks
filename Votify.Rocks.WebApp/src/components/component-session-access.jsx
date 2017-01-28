import React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import ContentAdd from 'material-ui/svg-icons/content/add';
import Dialog from 'material-ui/Dialog';
import FlatButton from 'material-ui/FlatButton';
import Toggle from 'material-ui/Toggle';

const styles = {
  toggle: {
    marginBottom: 16,
    marginTop: 16,
  },
};

class SessionAccess extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
             open: false,
             descriptionText: ''
         };
    };

    handleOpen () {
        this.setState({open: true});
    };

    handleClose () {
        console.log(this.descriptionTextField);
        this.props.onCreateClick();
        this.setState({open: false});
    };

    handleCancel () {
        this.setState({open: false});
    };

    render () {
        const {session, onSessionKeyChange, onDisplayNameChange, onEmailChange, onCreateClick, onJoinClick} = this.props;

            const actions = [
            <FlatButton
                label="Cancel"
                primary={false}
                keyboardFocused={false}
                onTouchTap={() => {this.handleCancel()}}
            />,
            <FlatButton
                label="Create"
                primary={true}
                keyboardFocused={false}
                onTouchTap={() => {this.handleClose()}}
            />,
            ];

        if(session && !session.participantUid) {
        return (
        <div>
            <div className="session-access-container">
                <TextField floatingLabelText="Vote session Key" value={session.key} onChange={onSessionKeyChange} errorText="" /><br/>
                <TextField floatingLabelText="Nickname" floatingLabelFixed={true} hintText={session.randomName} value={session.displayName} onChange={onDisplayNameChange} /><br/>
                <RaisedButton label="Join" onTouchTap={onJoinClick} primary={true} />
            </div>
            <Dialog title="Create a new vote session" actions={actions} modal={true} open={this.state.open} onRequestClose={this.handleClose}>
                <TextField floatingLabelText="Email" floatingLabelFixed={true} hintText="name@domain.com" value={session.email} onChange={onEmailChange} fullWidth={true} /><br/>
                <TextField floatingLabelText="Nickname" floatingLabelFixed={true} hintText={session.randomName} value={session.displayName} onChange={onDisplayNameChange} fullWidth={true} /><br/>
                <TextField hintText="Describe your vote session in a few words" floatingLabelText="Description" fullWidth={true} multiLine={true} rows={2} ref={ ref => this.descriptionTextField = ref } /><br />
                <Toggle label="I don't need to vote" labelPosition="right" style={styles.toggle} />
            </Dialog>
            <FloatingActionButton className="floating-create-button" secondary={true} onTouchTap={() => {this.handleOpen()}}>
                <ContentAdd />
            </FloatingActionButton>
        </div>
        )}

        return null;
    };
}

export default SessionAccess