function resetView() {
    ['confirmation', 'reception', 'agencies', 'contoso', 'fabrikam', 'woodgrove']
        .forEach(element => setStepState(element));
}

function setStepState(step, state) {
    const classesToRemove = ['disabled', 'active', 'completed'];
    document.getElementById(step).classList.remove(...classesToRemove);

    if (state) {
        document.getElementById(step).classList.add(state);

        if (state === 'disabled') {
            document.getElementById(step + '-icon').className = 'thumbs down icon';
        }

        if (state === 'completed') {
            document.getElementById(step + '-icon').className = 'thumbs up icon';
        }
    }
    else { // reset icon to original state
        const ogIcon = document.getElementById(step + '-icon').getAttribute('data-icon');
        document.getElementById(step + '-icon').className = ogIcon + ' icon';
    }
}

function registerEventHandlers(connection) {
    connection.on('loanApplicationStart', loanApplication => {
        resetView();
        document.getElementById('reception').classList.add('active');
    });

    connection.on('loanApplicationReceived', (loanApplication, result) => {
        if (result === true) {
            setStepState('reception', 'completed');
        }
        else {
            setStepState('reception', 'disabled');
        }
    });

    connection.on('agencyCheckPhaseStarted', () => {
        setStepState('agencies', 'active');
    });

    connection.on('agencyCheckPhaseCompleted', result => {
        if (result === true) {
            setStepState('agencies', 'completed');
        }
        else {
            setStepState('agencies', 'disabled');
        }
    });

    connection.on('agencyCheckStarted', request => {
        setStepState(request.AgencyId, 'active');
    });

    connection.on('agencyCheckComplete', result => {
        if (result.IsApproved === true) {
            setStepState(result.AgencyId, 'completed');
        }
        else {
            setStepState(result.AgencyId, 'disabled');
        }
    });

    connection.on('loanApplicationComplete', result => {
        if (result.IsApproved === true) {
            setStepState('confirmation', 'completed');
        }
        else {
            setStepState('confirmation', 'disabled');
        }
    });
}

function buildHubConnection(hubUrl) {
    return new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withAutomaticReconnect([0, 0, 10000])
        .configureLogging(signalR.LogLevel.Information)
        .build();
}

async function start(hubUrl) {
    try {
        const connection = buildHubConnection(hubUrl);
        registerEventHandlers(connection);
        await connection.start();
        console.assert(connection.state === signalR.HubConnectionState.Connected);
        console.log('connected');
    } catch (err) {
        console.assert(connection.state === signalR.HubConnectionState.Disconnected);
        console.error(err);
        setTimeout(() => start(), 5000);
    }
}
