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

export class CreateVoteSession extends React.Component { 
    constructor(props) {
        super(props);
        this.state = {
            stepIndex: 0,
            nextButtonText: 'Next',
         };
         this.handleDialogNextClick = this.handleDialogNextClick.bind(this);
         this.handleJumpToStep = this.handleJumpToStep.bind(this);
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
            <div>
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
                                ref={(e) => this.emailInput = e}
                                floatingLabelText="" 
                                floatingLabelFixed={true} 
                                hintText={emailHintText} 
                                value={session.email} 
                                onChange={onEmailChange} 
                                fullWidth={true} 
                                errorText={emailErrorText} 
                                />
                        </StepContent>
                    </Step>
                    <Step>
                        <StepButton onTouchTap={() => this.handleJumpToStep(1)}>
                        Nickname
                        </StepButton>
                        <StepContent>
                            <TextField 
                                floatingLabelText="" 
                                floatingLabelFixed={true} 
                                hintText={session.randomName} 
                                value={session.displayName} 
                                onChange={onDisplayNameChange} 
                                fullWidth={true} 
                                />
                        </StepContent>
                    </Step>
                    <Step>
                        <StepButton onTouchTap={() => this.handleJumpToStep(2)}>
                        Session description
                        </StepButton>
                        <StepContent>
                            <TextField 
                                hintText="Describe your vote session in a few words" 
                                floatingLabelText="" 
                                 floatingLabelFixed={true}
                                fullWidth={true} 
                                multiLine={true} 
                                rows={2}
                                value={session.description}
                                onChange={onDescriptionChange}
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
                        label="Cancel"
                        primary={false}
                        keyboardFocused={false}
                        onTouchTap={onCancel}
                    />
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