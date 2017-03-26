export const userCookieNamey = 'VR_USER';
export const participantUid = 'VR_PARTICIPANT_UID';

const devMode = false;
export default {
    apiUrl: devMode ? 'http://localhost/votifyrocks' : 'http://votifyrocksservicewebhost.azurewebsites.net/',
};