# NBi documentation

The Jekyll site is built from this directory with the same Just the Docs theme
as Expressif. The NBi homepage uses its existing landing layout. Guide pages
are in `guide/`, automation and GenbiL pages are in `automation/`, and older
release pages are in `downloads/`. Keep the `permalink` when moving an existing
page so links to the published site keep working.

## Preview locally

Run this from the repository root with Docker Desktop in Linux container mode.

```powershell
docker run --rm -p 4000:4000 --mount "type=bind,source=$((Get-Location).Path)/docs,target=/site" --mount "type=volume,source=nbi-docs-bundle,target=/usr/local/bundle" -w /site ruby:3.3 sh -c 'bundle install && bundle exec jekyll serve --host 0.0.0.0 --disable-disk-cache --force_polling'
```

Open <http://localhost:4000>.

## Publish

After the migration is merged, set **Settings → Pages → Build and deployment →
Source** to **GitHub Actions** and keep `www.nbi.io` as the custom domain. Run
the **Pages** workflow on `main` for the first deployment. Later changes to the
site on `main` deploy automatically.
