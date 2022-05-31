echo "build start"

./scripts/fastlane-init.sh

echo "--- Building"
bundle exec fastlane ios deploy_dev
echo "~~~ End Build"
