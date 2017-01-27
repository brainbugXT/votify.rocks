import _globals from '../globals'
import {post, get} from './service-core'

const serviceUrl = _globals.apiUrl;

export const CreateVoteSession = (participant, description, signalRClientId) => {
    const route = '/Create';
    const url = serviceUrl+route;
    
    const queryStringParams = { description: description, signalRClientId: signalRClientId };

    const postData = {
        displayName: participant.displayName,
        isOrganizer: participant.isOrganizer,
        canVote: participant.canVote
    };

    return post( url, queryStringParams, postData);
};

export const JoinVoteSession = (voteSessionKey, participant,  signalRClientId) => {
    const route = `/${voteSessionKey}/Join`;
    const url = serviceUrl+route;
    
    const queryStringParams = { signalRClientId: signalRClientId };

    const postData = {
        displayName: participant.displayName,
        isOrganizer: participant.isOrganizer,
        canVote: participant.canVote
    };

    return post( url, queryStringParams, postData);
};

export const LeaveVoteSession = (voteSessionKey, participantUid,  signalRClientId) => {
    const route = `/${voteSessionKey}/${participantUid}/Leave`;
    const url = serviceUrl+route;

    const queryStringParams = { signalRClientId: signalRClientId };

    return post( url, queryStringParams);
};

export const CastVote = (voteSessionKey, participantUid, voteValue) => {
    const route = `/${voteSessionKey}/${participantUid}/Vote`;
    const url = serviceUrl+route;

    const queryStringParams = { value: voteValue };
    return post( url, queryStringParams);
};

export const OpenVoteSession = (voteSessionKey, participantUid) => {
    const route = `/${voteSessionKey}/Open`;
    const url = serviceUrl+route;
    
    const queryStringParams = { participantUid: participantUid };

    return post( url, queryStringParams);
};

export const RandomName = () => {
    const route = `/RandomName/Generate`;
    const url = serviceUrl+route;

    return get( url, {});
};