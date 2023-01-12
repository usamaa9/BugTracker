using PurpleRock.BugTracker.WebApi;

CommonHostApi.Run<Startup>(
    args,
    () => { },
    builder => builder);
