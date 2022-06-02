echo "build start"

cd fastlane

# rbenv
eval "$(rbenv init -)"

bundle install

# fastlane
export LC_ALL=en_US.UTF-8
export LANG=en_US.UTF-8

echo "--- Building"
bundle exec fastlane android deploy_dev job:build-apk
echo "~~~ End Build"
