import React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import ContentAdd from 'material-ui/svg-icons/content/add';
import Dialog from 'material-ui/Dialog';
import FlatButton from 'material-ui/FlatButton';
import Toggle from 'material-ui/Toggle';
import { Step, Stepper, StepButton, StepContent, } from 'material-ui/Stepper';

const styles = {
  toggle: {
    marginBottom: 16,
    marginTop: 16,
  },
};
const ENTER_KEY_CODE = 13;

export class CreateVoteSession extends React.Component { 
    constructor(props) {
        super(props);
        this.state = {
            stepIndex: 0,
            nextButtonText: 'Next',
         };
         this.handleDialogNextClick = this.handleDialogNextClick.bind(this);
         this.handleJumpToStep = this.handleJumpToStep.bind(this);
         this.handleEnterKey = this.handleEnterKey.bind(this);
    };

    handleDialogNextClick() {
        const {stepIndex} = this.state;
        if(this.props.validateEmail()){
            if(stepIndex < 3){
                this.setState({stepIndex: stepIndex + 1});
                this.setState({nextButtonText: 'Next'});
            }
            else {
                this.setState({nextButtonText: 'Create'});
            }

            if(stepIndex === 3){
                this.props.onRequestClose();
            }
        }
        
    }

    handleEnterKey(e) {
        if(e.keyCode === ENTER_KEY_CODE){
            this.handleDialogNextClick();
        }
    }

    focusInputField(textInput) {
        if(textInput){
            textInput.focus();
        }
    };

    handleJumpToStep(step) {
        if(this.props.validateEmail()){
            this.setState({stepIndex: step});
        }
    }

    render(){
        const {open, onRequestClose, onCancel, session, onDisplayNameChange, onEmailChange, onDescriptionChange, onCanVoteChange, emailErrorText, emailHintText, validateEmail,} = this.props;

        if(!open){
            return null;
        }

        return (
            <div style={{minHeight: 400}}>
                <Stepper
                    activeStep={this.state.stepIndex}
                    linear={false}
                    orientation="vertical"
                    >
                    <Step >
                        <StepButton onTouchTap={() => this.handleJumpToStep(0)}>
                        Email address
                        </StepButton>
                        <StepContent>
                            <TextField 
                                ref={this.focusInputField}
                                floatingLabelText="" 
                                floatingLabelFixed={true} 
                                hintText={emailHintText} 
                                value={session.email} 
                                onChange={onEmailChange} 
                                fullWidth={true} 
                                errorText={emailErrorText} 
                                onKeyDown={this.handleEnterKey}
                                />
                        </StepContent>
                    </Step>
                    <Step>
                        <StepButton onTouchTap={() => this.handleJumpToStep(1)}>
                        Nickname
                        </StepButton>
                        <StepContent>
                            <TextField 
                                ref={this.focusInputField}
                                floatingLabelText="" 
                                floatingLabelFixed={true} 
                                hintText={session.randomName} 
                                value={session.displayName} 
                                onChange={onDisplayNameChange} 
                                fullWidth={true} 
                                onKeyDown={this.handleEnterKey}
                                />
                        </StepContent>
                    </Step>
                    <Step>
                        <StepButton onTouchTap={() => this.handleJumpToStep(2)}>
                        Session description
                        </StepButton>
                        <StepContent>
                            <TextField 
                                ref={this.focusInputField}
                                hintText="Describe your vote session in a few words" 
                                floatingLabelText="" 
                                floatingLabelFixed={true}
                                fullWidth={true} 
                                multiLine={true} 
                                rows={2}
                                value={session.description}
                                onChange={onDescriptionChange}
                                onKeyDown={this.handleEnterKey}
                                />
                        </StepContent>
                    </Step>
                    <Step>
                        <StepButton onTouchTap={() => this.handleJumpToStep(3)}>
                        Voting options
                        </StepButton>
                        <StepContent>
                            <Toggle label="I don't need to vote" labelPosition="right" style={styles.toggle} />
                        </StepContent>
                    </Step>
                    </Stepper>
                    <div className="action-buttons"></div>
                    <FlatButton
                        label={this.state.nextButtonText}
                        primary={true}
                        keyboardFocused={false}
                        onTouchTap={this.handleDialogNextClick}
                    />
                </div>
        )
    }
    };

export default CreateVoteSession