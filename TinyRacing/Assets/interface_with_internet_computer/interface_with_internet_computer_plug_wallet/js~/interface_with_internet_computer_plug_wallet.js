mergeInto(LibraryManager.library, {

  plug_wallet_request_connect: function (call_back) {
    console.log(`[JavaScript] plug_wallet_request_connect`);
    // let stateCheck = setInterval(() => {
    //   if (document.readyState === 'complete') {
    //     clearInterval(stateCheck);
    //   }
    // }, 100);
    const verifyConnectionAndAgent = async () => {
      if (
        typeof window.ic === 'undefined' ||
        typeof window.ic.plug === 'undefined') {
        window.open("https://plugwallet.ooo/", '_blank');
        return false;
      }
      let connected = await window.ic.plug.isConnected();
      if (!connected) {
        connected = await window.ic.plug.requestConnect();
      }
      if (connected && !window.ic.plug.agent) {
        await window.ic.plug.createAgent();
      }
      return connected;
    }
    (async () => {
      const connected = await verifyConnectionAndAgent();
      console.log(`[JavaScript] plug wallet connected ${connected}`);
      const principal_identifier = connected ?
        `${await window.ic.plug.agent.getPrincipal()}` : // workaround for empty string on C# side ...
        ``;
      const buffer_size = 64; //lengthBytesUTF8(principal_identifier) + 1; not working...
      console.log(`[JavaScript] principal_identifier ${principal_identifier}, buffer size ${buffer_size}`);
      let buffer = _malloc(buffer_size);
      stringToUTF8(principal_identifier, buffer, buffer_size);
      dynCall_vi(call_back, buffer); // out void in pointer
      _free(buffer);
    })();
  },


});