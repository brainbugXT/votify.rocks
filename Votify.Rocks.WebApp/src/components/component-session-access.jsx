import React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import ContentAdd from 'material-ui/svg-icons/content/add';
import Dialog from 'material-ui/Dialog';
import FlatButton from 'material-ui/FlatButton';
import Toggle from 'material-ui/Toggle';

const emailHintText = 'name@example.com';

const styles = {
  toggle: {
    marginBottom: 16,
    marginTop: 16,
  },
};

const validateEmail = (email) => {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

class SessionAccess extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
             createDialogOpen: false,
             joinDialogOpen: false,
             joinKeyErrorText: '',
             emailErrorText: '',
             descriptionText: ''
         };
    };

    handleCreateDialogOpen () {
        this.setState({createDialogOpen: true});
    };

    handleCreateDialogClose () {
        if(!this.props.session.email){
            this.setState({emailErrorText: 'Required'});
            return;
        } else if(!validateEmail(this.props.session.email)){
            this.setState({emailErrorText: 'Check format "'+emailHintText+'"'});
            return;
        }
        this.setState({emailErrorText: ''});
        this.props.onCreateClick();
        this.setState({createDialogOpen: false});
    };

    handleCreateDialogCancel () {
        this.setState({emailErrorText: ''});
        this.setState({createDialogOpen: false});
    };

    handleJoinClick () {
        if(!this.props.session.key){
            this.setState({joinKeyErrorText: 'Required'});
            return;
        }
        this.setState({joinKeyErrorText: ''});
        this.setState({joinDialogOpen: true});
    };

    handleJoinDialogClose () {
        this.props.onJoinClick();
        this.setState({joinDialogOpen: false});
    };

    handleJoinDialogCancel () {
        this.setState({joinDialogOpen: false});
    };

    render () {
        const {session, onSessionKeyChange, onDisplayNameChange, onEmailChange, onCreateClick, onJoinClick} = this.props;

            const createActions = [
            <FlatButton
                label="Cancel"
                primary={false}
                keyboardFocused={false}
                onTouchTap={() => {this.handleCreateDialogCancel()}}
            />,
            <FlatButton
                label="Create"
                primary={true}
                keyboardFocused={false}
                onTouchTap={() => {this.handleCreateDialogClose()}}
            />,
            ];

        const joinActions = [
            <FlatButton
                label="Cancel"
                primary={false}
                keyboardFocused={false}
                onTouchTap={() => {this.handleJoinDialogCancel()}}
            />,
            <FlatButton
                label="Ok"
                primary={true}
                keyboardFocused={false}
                onTouchTap={() => {
                    this.handleJoinDialogClose();
                    onJoinClick();
                    }}
            />,
            ];

        if(session && !session.participantUid) {
        return (
        <div>
            <div className="session-access-container">
                <TextField floatingLabelText="Vote session Key" value={session.key} onChange={onSessionKeyChange} errorText={this.state.joinKeyErrorText} /><br/>
                <RaisedButton label="Join" onTouchTap={() => this.handleJoinClick()} primary={true} />
            </div>
            <Dialog title={'Join vote session ' + session.key} actions={joinActions} modal={true} open={this.state.joinDialogOpen} onRequestClose={this.handleJoinDialogClose}>
                <p>We need a nickname so that other participants know who you are, or you can just use the cool random name we have assigned below...</p>
                <TextField floatingLabelText="Nickname" floatingLabelFixed={true} hintText={session.randomName} value={session.displayName} onChange={onDisplayNameChange} fullWidth={true} />
            </Dialog>
            <Dialog title="Create a new vote session" actions={createActions} modal={true} open={this.state.createDialogOpen} onRequestClose={this.handleCreateDialogClose}>
                <TextField floatingLabelText="Email" floatingLabelFixed={true} hintText={emailHintText} value={session.email} onChange={onEmailChange} fullWidth={true} errorText={this.state.emailErrorText} /><br/>
                <TextField floatingLabelText="Nickname" floatingLabelFixed={true} hintText={session.randomName} value={session.displayName} onChange={onDisplayNameChange} fullWidth={true} /><br/>
                <TextField hintText="Describe your vote session in a few words" floatingLabelText="Description" fullWidth={true} multiLine={true} rows={2} ref={ ref => this.descriptionTextField = ref } /><br />
                <Toggle label="I don't need to vote" labelPosition="right" style={styles.toggle} />
            </Dialog>
            <FloatingActionButton className="floating-create-button" secondary={true} onTouchTap={() => {this.handleCreateDialogOpen()}}>
                <ContentAdd />
            </FloatingActionButton>
        </div>
        )}

        return null;
    };
}

export default SessionAccess