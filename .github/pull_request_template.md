## Summary

Describe the change and the user or maintainer outcome it improves.

## Checklist

- [ ] I ran the relevant local checks
- [ ] I updated docs or examples when behavior changed
- [ ] I kept secrets and machine-specific values out of source control
- [ ] I verified the Docker / Coolify flow still makes sense for Linux deployers

## Validation

List the commands or checks you ran, for example:

```text
dotnet test
dotnet build Umbracool.sln -c Release
docker compose -f docker-compose.coolify.yml config
```

## Notes

Anything reviewers should pay attention to.
