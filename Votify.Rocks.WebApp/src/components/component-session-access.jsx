import React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import ContentAdd from 'material-ui/svg-icons/content/add';
import Dialog from 'material-ui/Dialog';
import FlatButton from 'material-ui/FlatButton';
import Toggle from 'material-ui/Toggle';
import CreateVoteSession from './component-session-create'

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
         this.validateEmail = this.validateEmail.bind(this);
    };

    handleCreateDialogOpen () {
        this.setState({createDialogOpen: true});
    };

    handleCreateDialogClose () {
        this.setState({emailErrorText: ''});
        this.props.onCreateClick();
        this.setState({createDialogOpen: false});
    };

    validateEmail() {
        if(!this.props.session.email){
            this.setState({emailErrorText: 'Required'});
            return false;
        } else if(!validateEmail(this.props.session.email)){
            this.setState({emailErrorText: 'Check format "'+emailHintText+'"'});
            return false;
        }

        return true;
    }

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
        const {session, onSessionKeyChange, onDisplayNameChange, onEmailChange, onDescriptionChange, onCanVoteChange, onCreateClick, onJoinClick} = this.props;

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
                    }}
            />,
            ];

        if(session && !session.participantUid) {
        return (
        <div>
            {
                !this.state.createDialogOpen
                ? <div className="session-access-container">
                    <TextField floatingLabelText="Vote session Key" 
                        value={session.key} onChange={onSessionKeyChange} 
                        errorText={this.state.joinKeyErrorText} 
                        floatingLabelStyle={{left: 70}} 
                        floatingLabelShrinkStyle={{left:80}} 
                        inputStyle={{textAlign: 'center'}} 
                        />
                        <br/>
                    <RaisedButton label="Join" onTouchTap={() => this.handleJoinClick()} primary={true} />
                </div>
                : null
            }
            <Dialog 
                actions={joinActions} 
                modal={true} 
                open={this.state.joinDialogOpen} 
                onRequestClose={this.handleJoinDialogClose}>
                <p>We need a nickname so that other participants know who you are, or you can just use the cool random name we have assigned below...</p>
                <TextField 
                    floatingLabelText="Nickname" 
                    floatingLabelFixed={true} 
                    hintText={session.randomName} 
                    value={session.displayName} 
                    onChange={onDisplayNameChange}
                    fullWidth={true} 
                    />
            </Dialog>
            <div className="session-access-create-container">
                <CreateVoteSession 
                    open={this.state.createDialogOpen}
                    emailHintText={emailHintText}
                    emailErrorText={this.state.emailErrorText} 
                    session={session} 
                    onDisplayNameChange={onDisplayNameChange}
                    onEmailChange={onEmailChange}
                    onDescriptionChange={onDescriptionChange} 
                    onCanVoteChange={onCanVoteChange}
                    onRequestClose={() => this.handleCreateDialogClose()}
                    onCancel={() => this.handleCreateDialogCancel()}
                    validateEmail={this.validateEmail}
                    />
                </div>
                {
                    !this.state.createDialogOpen
                    ? <FloatingActionButton 
                        className="floating-create-button" 
                        secondary={true} 
                        onTouchTap={() => {this.handleCreateDialogOpen()}}>
                        <ContentAdd />
                    </FloatingActionButton>
                    : null
                }
        </div>
        )}

        return null;
    };
}

export default SessionAccess